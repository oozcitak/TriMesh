using System;

namespace TriMesh
{
    /// <summary>
    /// Represents a 2D location.
    /// </summary>
    public sealed class Vertex
    {
        /// <summary>
        /// Gets whether this vertex belongs to a super triangle.
        /// </summary>
        public bool IsSuperVertex { get; internal set; }
        /// <summary>
        /// Gets whether this vertex bwlongs to the input vertex set or inserted
        /// during mesh refinement.
        /// </summary>
        public bool IsInputVertex { get; internal set; }

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public double Y { get; private set; }
        /// <summary>
        /// Gets the array of user attributes. As far as mesh generation is concerned those
        /// are ignored. However, those attributes are linearly interpolated between vertices 
        /// when new vertices are inserted between existing ones.
        /// </summary>
        public double[] Attributes { get; private set; }

        /// <summary>
        /// Gets the origin vertex.
        /// </summary>
        public static Vertex Zero { get { return new Vertex(0, 0); } }

        /// <summary>
        /// Instantiates a new vertex.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public Vertex(double x, double y, params double[] attributes)
        {
            X = x;
            Y = y;
            Attributes = attributes;

            IsSuperVertex = false;
            IsInputVertex = false;
        }

        /// <summary>
        /// Returns the distance to the given vertex.
        /// </summary>
        /// <param name="other">The other vertex to measure the distance to.</param>
        /// <returns>Distance between the vertices.</returns>
        public double DistanceTo(Vertex other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static bool operator ==(Vertex a, Vector b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return Utility.AlmostEqual(a.X, b.X) && Utility.AlmostEqual(a.Y, b.Y);
        }

        public static bool operator !=(Vertex a, Vector b)
        {
            return !(a == b);
        }

        public static Vertex operator +(Vertex a, Vector b)
        {
            return new Vertex(a.X + b.X, a.Y + b.Y);
        }

        public static Vertex operator -(Vertex a, Vector b)
        {
            return new Vertex(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator -(Vertex a, Vertex b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Interpolates an attribute between two vertices.
        /// </summary>
        /// <param name="a">The first vertex</param>
        /// <param name="b">The second vertex</param>
        /// <param name="aZ">Attribute of the first vertex</param>
        /// <param name="bZ">Attribute of the second vertex</param>
        /// <param name="x">X coordinate of the new location</param>
        /// <param name="y">Y coordinate of the new location</param>
        /// <returns>Interpolated attribute value at the new location</returns>
        public static double InterpolateAttribute(Vertex a, Vertex b, double aZ, double bZ, double x, double y)
        {
            double d2 = (b - a).Length;
            if (Utility.AlmostZero(d2)) return (aZ + bZ) / 2;

            Vertex v = new Vertex(x, y, 0);
            double d1 = (v - a).Length;

            return d1 / d2 * (bZ - aZ) + aZ;
        }

        /// <summary>
        /// Calculates the average of the given vertices.
        /// </summary>
        /// <param name="vertices">A list of vertices</param>
        /// <returns>The average of the given vertices</returns>
        public static Vertex Average(params Vertex[] vertices)
        {
            double n = vertices.Length;
            double x = 0, y = 0;
            foreach (Vertex v in vertices)
            {
                x += v.X;
                y += v.Y;
            }
            return new Vertex(x / n, y / n);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            Vertex other = obj as Vertex;
            if (other == null) return false;

            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked // Wrap if overflows
            {
                int hash = 17;

                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return X.ToString("F2") + ", " + Y.ToString("F2");
        }
    }
}
