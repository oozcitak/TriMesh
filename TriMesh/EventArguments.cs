using System;
using System.Collections.Generic;

namespace TriMesh
{
    /// <summary>
    /// Contains event data for vertex insertion.
    /// </summary>
    public class InsertVertexEventArgs : EventArgs
    {
        /// <summary>
        /// The new vertex inserted into the mesh.
        /// </summary>
        public Vertex NewVertex { get; private set; }

        public InsertVertexEventArgs(Vertex vertex)
        {
            NewVertex = vertex;
        }
    }

    /// <summary>
    /// Contains event data for the state before triangle divison.
    /// </summary>
    public class DividingTriangleEventArgs : EventArgs
    {
        /// <summary>
        /// The new vertex being inserted.
        /// </summary>
        public Vertex NewVertex { get; private set; }
        /// <summary>
        /// The triangles that will be divided.
        /// </summary>
        public IEnumerable<Triangle> Triangles { get; private set; }

        public DividingTriangleEventArgs(Vertex vertex, IEnumerable<Triangle> tri)
        {
            NewVertex = vertex;
            Triangles = tri;
        }
    }

    /// <summary>
    /// Contains event data for the state after triangle divison.
    /// </summary>
    public class DividedTriangleEventArgs : EventArgs
    {
        /// <summary>
        /// The new vertex being inserted.
        /// </summary>
        public Vertex NewVertex { get; private set; }
        /// <summary>
        /// The triangles that were divided.
        /// </summary>
        public IEnumerable<Triangle> DividedTriangles { get; private set; }
        /// <summary>
        /// The new triangles that were added to the mesh.
        /// </summary>
        public IEnumerable<Triangle> NewTriangles { get; private set; }

        public DividedTriangleEventArgs(Vertex vertex, IEnumerable<Triangle> divided, IEnumerable<Triangle> added)
        {
            NewVertex = vertex;
            DividedTriangles = divided;
            NewTriangles = added;
        }
    }

    /// <summary>
    /// Contains event data for the state before edge flipping.
    /// </summary>
    public class FlippingEdgeEventArgs : EventArgs
    {
        /// <summary>
        /// The common edge between bad triangles.
        /// </summary>
        public Halfedge Edge { get; private set; }
        /// <summary>
        /// The first triangle whose apex is encroaching on the edge.
        /// </summary>
        public Triangle BadTriangle1 { get; private set; }
        /// <summary>
        /// The second triangle whose apex is encroaching on the edge.
        /// </summary>
        public Triangle BadTriangle2 { get; private set; }

        public FlippingEdgeEventArgs(Halfedge edge, Triangle t1, Triangle t2)
        {
            Edge = edge;
            BadTriangle1 = t1;
            BadTriangle2 = t2;
        }
    }

    /// <summary>
    /// Contains event data for the state after edge flipping.
    /// </summary>
    public class FlippedEdgeEventArgs : EventArgs
    {
        /// <summary>
        /// The new edge between flipped triangles.
        /// </summary>
        public Halfedge Edge { get; private set; }
        /// <summary>
        /// The first flipped triangle.
        /// </summary>
        public Triangle NewTriangle1 { get; private set; }
        /// <summary>
        /// The second flipped triangle.
        /// </summary>
        public Triangle NewTriangle2 { get; private set; }

        public FlippedEdgeEventArgs(Halfedge edge, Triangle t1, Triangle t2)
        {
            Edge = edge;
            NewTriangle1 = t1;
            NewTriangle2 = t2;
        }
    }

    public delegate void InsertVertexEventHandler(object sender, InsertVertexEventArgs e);
    public delegate void DividingTriangleEventHandler(object sender, DividingTriangleEventArgs e);
    public delegate void DividedTriangleEventHandler(object sender, DividedTriangleEventArgs e);
    public delegate void FlippingEdgeEventHandler(object sender, FlippingEdgeEventArgs e);
    public delegate void FlippedEdgeEventHandler(object sender, FlippedEdgeEventArgs e);
}
