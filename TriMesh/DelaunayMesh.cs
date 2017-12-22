using System;
using System.Collections.Generic;
using System.Linq;

namespace TriMesh
{
    /// <summary>
    /// Represents a two-dimensional dealunay mesh created from a set of input vertices
    /// </summary>
    public class DelaunayMesh
    {
        public event InsertVertexEventHandler InsertVertex;
        public event DividingTriangleEventHandler DividingTriangle;
        public event DividedTriangleEventHandler DividedTriangle;
        public event FlippingEdgeEventHandler FlippingEdge;
        public event FlippedEdgeEventHandler FlippedEdge;

        private Extents limits = new Extents();
        private Triangle superTriangle = null;
        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Gets the collection of input vertices.
        /// </summary>
        public VertexCollection InputVertices { get; private set; }
        /// <summary>
        /// Gets the collection of mesh vertices.
        /// </summary>
        public VertexCollection Vertices { get; private set; }
        /// <summary>
        /// Gets the collection of mesh triangles.
        /// </summary>
        public TriangleCollection Triangles { get; private set; }
        /// <summary>
        /// Gets the time elapsed for the last triangulation.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }
        /// <summary>
        /// Gets the time elapsed for the last triangulation including
        /// time spent in event handlers.
        /// </summary>
        public TimeSpan ElapsedTimeWithEvents { get; private set; }

        public DelaunayMesh()
        {
            InputVertices = new VertexCollection();
        }

        /// <summary>
        /// Adds a new vertex into input vertices
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <returns>The new vertex</returns>
        public Vertex AddVertex(double x, double y, double z)
        {
            Vertex v = new Vertex(x, y, z);
            InputVertices.Add(v);
            limits.Add(x, y);
            return v;
        }

        /// <summary>
        /// Adds a new vertex into input vertices
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>The new vertex</returns>
        public Vertex AddVertex(double x, double y)
        {
            return AddVertex(x, y, 0);
        }

        /// <summary>
        /// Triangulates the mesh
        /// </summary>
        public void Triangulate()
        {
            System.Diagnostics.Stopwatch swEvents = new System.Diagnostics.Stopwatch();
            sw.Start();
            swEvents.Start();

            InitMesh();
            CreateSuperTriangle();
            InsertInputVertices();
            RemoveSuperTriangles();

            sw.Stop();
            swEvents.Stop();

            ElapsedTime = sw.Elapsed;
            ElapsedTimeWithEvents = swEvents.Elapsed;
        }

        /// <summary>
        /// Initilizes storage
        /// </summary>
        private void InitMesh()
        {
            Vertices = new VertexCollection();
            Triangles = new TriangleCollection();
        }

        /// <summary>
        /// Cerates the suoer-triangle encompassing all input vertices
        /// </summary>
        private void CreateSuperTriangle()
        {
            // Offset limits to enclose all input vertices
            Extents ex = limits.Offset(limits.Width / 5 + 1, limits.Height / 5 + 1);
            double x1 = ex.Xmin;
            double x2 = ex.Xmax;
            double y1 = ex.Ymin;
            double y2 = ex.Ymax;

            // Side length and height of the equilateral super-triangle
            double ad = (y2 - y1) / Math.Tan(Math.PI / 3);
            double a = (x2 - x1) + 2 * ad;
            double h = a / 2 * Math.Tan(Math.PI / 3);

            // Corner vertices of  the super triangle
            Vertex v1 = new Vertex(x1 - ad, y1, 0);
            Vertex v2 = new Vertex(x2 + ad, y1, 0);
            Vertex v3 = new Vertex((x1 + x2) / 2, y1 + h, 0);
            v1.IsSuperVertex = true;
            v2.IsSuperVertex = true;
            v3.IsSuperVertex = true;

            superTriangle = new Triangle(v1, v2, v3);
            Triangles.SetRootTriangle(superTriangle);
        }

        /// <summary>
        /// Removes triangles with super vertices
        /// </summary>
        private void RemoveSuperTriangles()
        {
            foreach (Triangle t in Triangles)
            {
                if (t.IsSuperTriangle) t.removed = true;
            }
        }

        /// <summary>
        /// Inserts all input vertices into the mesh
        /// </summary>
        private void InsertInputVertices()
        {
            foreach (Vertex v in InputVertices)
            {
                InsertInputVertex(v);
            }
        }

        /// <summary>
        /// Inserts a new vertex into the triangulation keeping the mesh
        /// locally delaunay by performing edge swaps.
        /// </summary>
        /// <param name="v">The new vertex</param>
        private void InsertInputVertex(Vertex v)
        {
            // Find the triangle(s) containing this vertex
            TriangleCollection.FindTriangle[] toDivide = Triangles.FindContaining(v);

            if (toDivide.Length == 1)
            {
                // Divide into three
                DivideTriangleAtVertex(toDivide[0].Triangle, v);
            }
            else if (toDivide.Length == 2)
            {
                // Divide into four
                DivideTrianglesOnSharedEdge(toDivide[0].Triangle, toDivide[0].Location, toDivide[1].Triangle, toDivide[1].Location, v);
            }
            else
            {
                throw new InvalidOperationException("Input vertex should coincide with one or two triangles.");
            }
            v.IsInputVertex = true;
            Vertices.Add(v);

            OnInsertVertex(new InsertVertexEventArgs(v));
        }

        /// <summary>
        /// Divide a triangle into three triangles at the given vertex
        /// performing local edge swaps as necessary to keep the mesh
        /// delaunay.
        /// </summary>
        /// <param name="t">The triangle to divide</param>
        /// <param name="v">The vertex inside the triangle</param>
        private void DivideTriangleAtVertex(Triangle t, Vertex v)
        {
            OnDividingTriangle(new DividingTriangleEventArgs(v, new Triangle[] { t }));

            Triangle t1 = new Triangle(t.V1, t.V2, v);
            Triangle t2 = new Triangle(t.V2, t.V3, v);
            Triangle t3 = new Triangle(t.V3, t.V1, v);

            t1.SetMeshParams(t.S12.Opposite, t2.S31, t3.S23);
            t2.SetMeshParams(t.S23.Opposite, t3.S31, t1.S23);
            t3.SetMeshParams(t.S31.Opposite, t1.S31, t2.S23);

            t.removed = true;
            Triangles.SetRootTriangle(t1);

            OnDividedTriangle(new DividedTriangleEventArgs(v, new Triangle[] { t }, new Triangle[] { t1, t2, t3 }));

            // edge flip
            SwapTest(t1.S12, v);
            SwapTest(t2.S12, v);
            SwapTest(t3.S12, v);
        }

        /// <summary>
        /// Divide two triangles at the given vertex located at
        /// their shared side into four new triangles performing local
        /// edge swaps as necessary to keep the mesh delaunay.
        /// </summary>
        /// <param name="tt1">First triangle to divide</param>
        /// <param name="loc1">The location of the vertex on the first triangle</param>
        /// <param name="tt2">Second triangle to divide</param>
        /// <param name="loc2">The location of the vertex on the second triangle</param>
        /// <param name="v">The vertex on the shared side</param>
        private void DivideTrianglesOnSharedEdge(Triangle tt1, PointOnTriangle loc1, Triangle tt2, PointOnTriangle loc2, Vertex v)
        {
            OnDividingTriangle(new DividingTriangleEventArgs(v, new Triangle[] { tt1, tt2 }));

            Halfedge e1 = (loc1 == PointOnTriangle.OnS12 ? tt1.S12 : (loc1 == PointOnTriangle.OnS23 ? tt1.S23 : tt1.S31));
            Halfedge e2 = (loc2 == PointOnTriangle.OnS12 ? tt2.S12 : (loc2 == PointOnTriangle.OnS23 ? tt2.S23 : tt2.S31));

            // Divide into four triangles
            Triangle[] tn1 = DivideTriangleOnEdge(tt1, loc1, v);
            Triangle[] tn2 = DivideTriangleOnEdge(tt2, loc2, v);

            Triangle t1 = tn1[0];
            Triangle t2 = tn1[1];
            Triangle t3 = tn2[0];
            Triangle t4 = tn2[1];
            t1.SetMeshParams(t4.S12, t2.S31, null);
            t2.SetMeshParams(t3.S12, null, t1.S23);
            t3.SetMeshParams(t2.S12, t4.S31, null);
            t4.SetMeshParams(t1.S12, null, t3.S23);

            tt1.removed = true;
            tt2.removed = true;
            Triangles.SetRootTriangle(t1);

            OnDividedTriangle(new DividedTriangleEventArgs(v, new Triangle[] { tt1, tt2 }, new Triangle[] { t1, t2, t3, t4 }));

            // edge flip
            SwapTest(t1.S31, v);
            SwapTest(t2.S23, v);
            SwapTest(t3.S31, v);
            SwapTest(t4.S23, v);
        }

        /// <summary>
        /// Divide a triangle at the given vertex located on a side
        /// without performing any edge swaps.
        /// </summary>
        /// <param name="t">The triangle to divide</param>
        /// <param name="loc">The location of the vertex on the triangle</param>
        /// <param name="v">The vertex on the side</param>
        private Triangle[] DivideTriangleOnEdge(Triangle t, PointOnTriangle loc, Vertex v)
        {
            Triangle t1 = null;
            Triangle t2 = null;

            if (loc == PointOnTriangle.OnS12)
            {
                // divide edge 12
                t1 = new Triangle(t.V1, v, t.V3);
                t2 = new Triangle(v, t.V2, t.V3);
                t1.SetMeshParams(t.S12.Opposite, t2.S31, t.S31.Opposite);
                t2.SetMeshParams(t.S12.Opposite, t.S23.Opposite, t1.S23);
            }
            else if (loc == PointOnTriangle.OnS23)
            {
                // divide edge 23
                t1 = new Triangle(t.V2, v, t.V1);
                t2 = new Triangle(v, t.V3, t.V1);
                t1.SetMeshParams(t.S23.Opposite, t2.S31, t.S12.Opposite);
                t2.SetMeshParams(t.S23.Opposite, t.S31.Opposite, t1.S23);
            }
            else if (loc == PointOnTriangle.OnS31)
            {
                // divide edge 31
                t1 = new Triangle(t.V3, v, t.V2);
                t2 = new Triangle(v, t.V1, t.V2);
                t1.SetMeshParams(t.S31.Opposite, t2.S31, t.S23.Opposite);
                t2.SetMeshParams(t.S31.Opposite, t.S12.Opposite, t1.S23);
            }

            return new Triangle[] { t1, t2 };
        }

        /// <summary>
        /// Checks two triangles sharing the given edge and flips the edge
        /// to ensure they are locally delaunay.
        /// </summary>
        /// <param name="e">Shared edge between two triangles</param>
        /// <param name="p">A vertex opposite the edge</param>
        /// <returns>true if the edge is swapped; false otherwise</returns>
        private bool SwapTest(Halfedge e, Vertex p)
        {
            if (e.Parent == null || e.Opposite == null || e.Opposite.Parent == null)
                return false;

            Triangle tri = e.Parent;
            Triangle otherTri = e.Opposite.Parent;
            Vertex a = e.V1;
            Vertex b = e.V2;
            Vertex d = e.Opposite.Next.V2;

            // delaunay edge, no flips required
            if (tri.Circumcircle.Contains(d) != PointShapeRelation.Inside)
                return false;

            OnFlippingEdge(new FlippingEdgeEventArgs(e, tri, otherTri));

            // set flipped flag
            tri.flipped = true;
            otherTri.flipped = true;

            // flip edge a-b to p-d
            Triangle tn1 = new Triangle(d, p, a);
            Triangle tn2 = new Triangle(p, d, b);
            tn1.SetMeshParams(tn2.S12, e.Prev.Opposite, e.Opposite.Next.Opposite);
            tn2.SetMeshParams(tn1.S12, e.Opposite.Prev.Opposite, e.Next.Opposite);

            Triangles.SetRootTriangle(tn1);

            OnFlippedEdge(new FlippedEdgeEventArgs(tn1.S12, tn1, tn2));

            SwapTest(tn1.S31, p);
            SwapTest(tn2.S23, p);

            return true;
        }

        protected virtual void OnInsertVertex(InsertVertexEventArgs e)
        {
            sw.Stop();
            InsertVertex?.Invoke(this, e);
            sw.Start();
        }

        protected virtual void OnDividingTriangle(DividingTriangleEventArgs e)
        {
            sw.Stop();
            DividingTriangle?.Invoke(this, e);
            sw.Start();
        }

        protected virtual void OnDividedTriangle(DividedTriangleEventArgs e)
        {
            sw.Stop();
            DividedTriangle?.Invoke(this, e);
            sw.Start();
        }

        protected virtual void OnFlippingEdge(FlippingEdgeEventArgs e)
        {
            sw.Stop();
            FlippingEdge?.Invoke(this, e);
            sw.Start();
        }

        protected virtual void OnFlippedEdge(FlippedEdgeEventArgs e)
        {
            sw.Stop();
            FlippedEdge?.Invoke(this, e);
            sw.Start();
        }
    }
}