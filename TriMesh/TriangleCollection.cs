using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TriMesh
{
    /// <summary>
    /// Represents a collection of triangles.
    /// </summary>
    public class TriangleCollection : IReadOnlyCollection<Triangle>
    {
        private Triangle root = null;
        private int iterationMarker = 0;

        /// <summary>
        /// Sets the root triangle to the givn one. The root triangle is used as
        /// a starting point in vertex searches and other iterations.
        /// </summary>
        /// <param name="t">New root triangle</param>
        internal void SetRootTriangle(Triangle t)
        {
            root = t;
        }

        /// <summary>
        /// Gets the number of valid (not removed nor flipped) triangles.
        /// </summary>
        public int Count { get { return this.Count(); } }

        /// <summary>
        /// Enumerates all valid (not removed nor flipped) triangles
        /// of the mesh.
        /// </summary>
        public IEnumerator<Triangle> GetEnumerator()
        {
            if (root != null)
            {
                iterationMarker++;
                Stack<Triangle> stack = new Stack<Triangle>();
                stack.Push(root);

                while (stack.Count > 0)
                {
                    Triangle tri = stack.Pop();

                    // yield return this triangle if it has not been visited before
                    if (!tri.removed && !tri.flipped && tri.mark != iterationMarker)
                    {
                        yield return tri;
                    }
                    tri.mark = iterationMarker;

                    // Add neighbours
                    if (tri.N12 != null && tri.N12.mark != iterationMarker) stack.Push(tri.N12);
                    if (tri.N23 != null && tri.N23.mark != iterationMarker) stack.Push(tri.N23);
                    if (tri.N31 != null && tri.N31.mark != iterationMarker) stack.Push(tri.N31);
                }
            }
        }

        /// <summary>
        /// Find the triangle(s) containing the given point. The search starts from an arbitrary
        /// triangle and homes in on the vertex by visiting triangles closest to the vertex.
        /// If the vertex is on and edge seperating two triangles, both are returned.
        /// </summary>
        /// <param name="v">The vertex to search for</param>
        /// <returns>The triangles(s) containing the vertex</returns>
        internal FindTriangle[] FindContaining(Vertex v)
        {
            Triangle current = root;
            while (true)
            {
                PointOnTriangle loc = PointOnTriangle.None;
                Halfedge closestEdge = null;
                PointShapeRelation rel = current.Contains(v, out loc, out closestEdge);
                if (rel == PointShapeRelation.Inside)
                {
                    // Inside triangle, return.
                    return new FindTriangle[] { new FindTriangle(current, loc) };
                }
                else if (rel == PointShapeRelation.Outside)
                {
                    // Outside triangle, continue searching from the neighbour closest to the point
                    Halfedge opp = closestEdge.Opposite;
                    if (opp == null)
                        throw new InvalidOperationException("No triangle contains this vertex.");
                    current = opp.Parent;
                }
                else
                {
                    // On an edge, return the triangle and its neighbour (if there is one)
                    Halfedge opp = closestEdge.Opposite;
                    if (opp == null)
                    {
                        return new FindTriangle[] { new FindTriangle(current, loc) };
                    }
                    else
                    {
                        Triangle other = opp.Parent;
                        PointOnTriangle otherLoc = PointOnTriangle.None;
                        other.Contains(v, out otherLoc);
                        return new FindTriangle[] { new FindTriangle(current, loc), new FindTriangle(other, otherLoc) };
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Represents search results with details.
        /// </summary>
        internal struct FindTriangle
        {
            public Triangle Triangle { get; private set; }
            public PointOnTriangle Location { get; private set; }

            public FindTriangle(Triangle tri, PointOnTriangle loc)
            {
                Triangle = tri;
                Location = loc;
            }
        }


    }
}
