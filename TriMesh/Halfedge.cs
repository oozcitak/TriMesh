using System;

namespace TriMesh
{
    /// <summary>
    /// Represents an edge of the mesh. There is also an opposing edge coincident
    /// with this one on the neighbouring triangle (except on the mesh boundary). Hence
    /// this is a half-edge between the two vertices, the other half being the opposing
    /// edge going in the opposite direction. Half-edges are oriented counterclockwise
    /// relative to their parent triangles.
    /// </summary>
    public sealed class Halfedge
    {
        private Vector dir = null;
        private Circle circumcircle = null;

        /// <summary>
        /// Gets the starting vertex.
        /// </summary>
        public Vertex V1 { get; private set; }
        /// <summary>
        /// Gets the ending vertex.
        /// </summary>
        public Vertex V2 { get; private set; }

        /// <summary>
        /// Gets the parent triangle.
        /// </summary>
        internal Triangle Parent { get; set; }
        /// <summary>
        /// Gets the previous half-edge of the parent triangle traveling
        /// clockwise to the previous vertex.
        /// </summary>
        internal Halfedge Prev { get; set; }
        /// <summary>
        /// Gets the next half-edge of the parent triangle traveling 
        /// counterclockwise to the next vertex.
        /// </summary>
        internal Halfedge Next { get; set; }
        /// <summary>
        /// Gets the opposite edge of the neighbouring triangle sharing the
        /// same vertices but running in the opposite direction.
        /// </summary>
        public Halfedge Opposite { get; set; }

        /// <summary>
        /// Gets the direction vector.
        /// </summary>
        public Vector Direction { get { if (dir == null) { dir = V2 - V1; } return dir; } }
        /// <summary>
        /// Gets the normal vector.
        /// </summary>
        public Vector Normal { get { return Direction.Normal; } }
        /// <summary>
        /// Gets the length of the edge.
        /// </summary>
        public double Length { get { return Direction.Length; } }

        /// <summary>
        /// Gets the circumscribed circle.
        /// </summary>
        public Circle Circumcircle
        {
            get
            {
                if (circumcircle == null)
                {
                    circumcircle = new Circle(Vertex.Average(V1, V2), V1.DistanceTo(V2) / 2);
                }
                return circumcircle;
            }
        }

        /// <summary>
        /// Instantiates a new edge object.
        /// </summary>
        /// <param name="v1">The starting vertex</param>
        /// <param name="v2">The ending vertex</param>
        public Halfedge(Vertex v1, Vertex v2)
        {
            V1 = v1;
            V2 = v2;

            Parent = null;
            Prev = null;
            Next = null;
            Opposite = null;
        }

        /// <summary>
        /// Sets the details about neighbouring edges in the mesh.
        /// </summary>
        /// <param name="parent">The triangle having this edge as a side.</param>
        /// <param name="prev">the previous half-edge of the parent triangle traveling
        /// clockwise to the previous vertex.</param>
        /// <param name="next">The next half-edge of the parent triangle traveling 
        /// counterclockwise to the next vertex</param>
        /// <param name="opposite">The opposite edge of the neighbouring triangle
        /// sharing the same vertices but running in the opposite direction.</param>
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
