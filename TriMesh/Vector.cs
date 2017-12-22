using System;

namespace TriMesh
{
    public sealed class Vector
    {
        private double? len2 = null;
        private double? len3 = null;
        private double? lensq2 = null;
        private double? lensq3 = null;

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public static Vector Zero { get { return new Vector(0, 0, 0); } }
        public static Vector UnitX { get { return new Vector(1, 0, 0); } }
        public static Vector UnitY { get { return new Vector(0, 1, 0); } }
        public static Vector UnitZ { get { return new Vector(0, 0, 1); } }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = Z;
        }

        public double LengthSquared2
        {
            get
            {
                if (lensq2 == null)
                {
                    lensq2 = X * X + Y * Y;
                }
                return lensq2.Value;
            }
        }

        public double LengthSquared3
        {
            get
            {
                if (lensq3 == null)
                {
                    lensq3 = X * X + Y * Y + Z * Z;
                }
                return lensq3.Value;
            }
        }

        public double Length2
        {
            get
            {
                if (len2 == null)
                {
                    len2 = Math.Sqrt(LengthSquared2);
                }
                return len2.Value;
            }
        }

        public double Length3
        {
            get
            {
                if (len3 == null)
                {
                    len3 = Math.Sqrt(LengthSquared3);
                }
                return len3.Value;
            }
        }

        public Vector Normalize()
        {
            return this / this.Length2;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector operator *(Vector a, double s)
        {
            return new Vector(a.X * s, a.Y * s, a.Z * s);
        }

        public static Vector operator /(Vector a, double s)
        {
            return new Vector(a.X / s, a.Y / s, a.Z / s);
        }

        public override string ToString()
        {
            return X.ToString("F2") + ", " + Y.ToString("F2") + ", " + Z.ToString("F2");
        }

        public static Vector Average(params Vector[] vectors)
        {
            double n = vectors.Length;
            double x = 0, y = 0, z = 0;
            foreach (Vector v in vectors)
            {
                x += v.X;
                y += v.Y;
                z += v.Z;
            }
            return new Vector(x / n, y / n, z / n);
        }
    }
}
