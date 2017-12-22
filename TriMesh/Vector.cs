using System;

namespace TriMesh
{
    public sealed class Vector
    {
        private double? len = null;
        private double? lensq = null;

        public double X { get; private set; }
        public double Y { get; private set; }

        public static Vector Zero { get { return new Vector(0, 0); } }
        public static Vector UnitX { get { return new Vector(1, 0); } }
        public static Vector UnitY { get { return new Vector(0, 1); } }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

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
