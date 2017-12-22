using System;

namespace TriMesh
{
    /// <summary>
    /// Represents a 2D displacement.
    /// </summary>
    public sealed class Vector
    {
        private double? len = null;
        private double? lensq = null;

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Gets the zero vector.
        /// </summary>
        public static Vector Zero { get { return new Vector(0, 0); } }
        /// <summary>
        /// Gets the unit vector along X axis.
        /// </summary>
        public static Vector UnitX { get { return new Vector(1, 0); } }
        /// <summary>
        /// Gets the unit vector along Y axis.
        /// </summary>
        public static Vector UnitY { get { return new Vector(0, 1); } }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the square of the magnitude of the vector.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                if (lensq == null)
                {
                    lensq = X * X + Y * Y;
                }
                return lensq.Value;
            }
        }

        /// <summary>
        /// Gets the magnitude of the vector.
        /// </summary>
        public double Length
        {
            get
            {
                if (len == null)
                {
                    len = Math.Sqrt(LengthSquared);
                }
                return len.Value;
            }
        }

        /// <summary>
        /// Returns the normalized vector.
        /// </summary>
        /// <returns></returns>
        public Vector Normalize()
        {
            return this / this.Length;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, double s)
        {
            return new Vector(a.X * s, a.Y * s);
        }

        public static Vector operator /(Vector a, double s)
        {
            return new Vector(a.X / s, a.Y / s);
        }

        public override string ToString()
        {
            return X.ToString("F2") + ", " + Y.ToString("F2");
        }

        /// <summary>
        /// Calculates the average of the given vectors.
        /// </summary>
        /// <param name="vectors">A list of vectors</param>
        /// <returns>The average of the given vectors</returns>
        public static Vector Average(params Vector[] vectors)
        {
            double n = vectors.Length;
            double x = 0, y = 0;
            foreach (Vector v in vectors)
            {
                x += v.X;
                y += v.Y;
            }
            return new Vector(x / n, y / n);
        }
    }
}
