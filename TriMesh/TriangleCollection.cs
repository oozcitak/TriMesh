using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TriMesh
{
    public class TriangleCollection : IReadOnlyCollection<Triangle>
    {
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

        private Triangle root = null;
        private int iterationMarker = 0;

        internal TriangleCollection(Triangle rootTriangle)
        {
            root = rootTriangle;
        }

        public int Count { get { return this.Count(); } }

        public IEnumerator<Triangle> GetEnumerator()
        {
            foreach (Triangle t in RawList())
            {
                if (!t.removed && !t.flipped)
                    yield return t;
            }
        }

        internal IEnumerable<Triangle> RawList()
        {
            if (root != null)
            {
                foreach (Triangle t in RawList(root))
                    yield return t;
            }
        }

        internal IEnumerable<Triangle> RawList(Triangle root)
        {
            iterationMarker++;
            Stack<Triangle> stack = new Stack<Triangle>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Triangle tri = stack.Pop();
                if (tri.mark != iterationMarker)
                {
                    tri.mark = iterationMarker;
                    yield return tri;
                }

                foreach (Triangle child in tri.children)
                {
                    stack.Push(child);
                }
            }
        }

        internal FindTriangle[] FindContaining(Vertex v)
        {
            Triangle current = null;
            foreach (Triangle t in RawList())
            {
                if (!t.removed && !t.flipped && t.children.Count == 0)
                {
                    current = t;
                    break;
                }
            }
            while (true)
            {
                PointOnTriangle loc = PointOnTriangle.None;
                Halfedge closestEdge = null;
                PointShapeRelation rel = current.Contains(v, out loc, out closestEdge);
                if (rel == PointShapeRelation.Inside)
                {
                    return new FindTriangle[] { new FindTriangle(current, loc) };
                }
                else if (rel == PointShapeRelation.Outside)
                {
                    Halfedge opp = closestEdge.Opposite;
                    if (opp == null)
                        throw new InvalidOperationException("No triangle contains this vertex.");
                    current = opp.Parent;
                }
                else // On an edge
                {
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
    }
}
