using System;

namespace TriMesh
{
    public sealed class Halfedge
    {
        private Vector dir = null;
        private Vector normal = null;
        private Circle circumcircle = null;

        public Vertex V1 { get; private set; }
        public Vertex V2 { get; private set; }

        internal Triangle Parent = null;
        internal Halfedge Prev = null;
        internal Halfedge Next = null;
        public Halfedge Opposite = null;

        public Vector Direction { get { if (dir == null) { dir = V2 - V1; } return dir; } }
        public Vector Normal { get { if (normal == null) { normal = new Vector(V2.Y - V1.Y, V1.X - V2.X, 0).Normalize(); } return normal; } }

        public double Length2 { get { return Direction.Length2; } }
        public double Length3 { get { return Direction.Length3; } }

        public Circle Circumcircle
        {
            get
            {
                if (circumcircle == null)
                {
                    circumcircle = new Circle(Vertex.Average(V1, V2), V1.DistanceTo2(V2) / 2);
                }
                return circumcircle;
            }
        }

        public Halfedge(Vertex v1, Vertex v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public Halfedge Offset(double delta)
        {
            Vector disp = Normal * delta;
            return new Halfedge(V1 + disp, V2 + disp);
        }

        internal void SetMeshParams(Triangle parent, Halfedge prev, Halfedge next, Halfedge opposite)
        {
            Parent = parent;
            Prev = prev;
            Next = next;
            Opposite = opposite;
            if (opposite != null) opposite.Opposite = this;
        }

        public override string ToString()
        {
            return "(" + V1.ToString() + "), (" + V2.ToString() + ")";
        }
    }
}
