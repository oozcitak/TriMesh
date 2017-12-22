using System;

namespace TriMesh
{
    public sealed class Matrix2
    {
        private double? determinant = null;

        public double A11 { get; private set; }
        public double A12 { get; private set; }
        public double A21 { get; private set; }
        public double A22 { get; private set; }

        public static Matrix2 Zero { get { return new Matrix2(0, 0, 0, 0); } }
        public static Matrix2 Identity { get { return new Matrix2(1, 0, 0, 1); } }

        public double Determinant
        {
            get
            {
                if (determinant == null)
                {
                    determinant = A11 * A22 - A21 * A12;
                }
                return determinant.Value;
            }
        }

        public Matrix2(double a11, double a12, double a21, double a22)
        {
            A11 = a11; A12 = a12;
            A21 = a21; A22 = a22;
        }

        public static Matrix2 operator +(Matrix2 a, Matrix2 b)
        {
            return new Matrix2(
                a.A11 + b.A11, a.A12 + b.A12,
                a.A21 + b.A21, a.A22 + b.A22);
        }

        public static Matrix2 operator -(Matrix2 a, Matrix2 b)
        {
            return new Matrix2(
                a.A11 - b.A11, a.A12 - b.A12,
                a.A21 - b.A21, a.A22 - b.A22);
        }

        public static Matrix2 operator *(Matrix2 a, Matrix2 b)
        {
            return new Matrix2(
                a.A11 * b.A11 + a.A12 * b.A21, a.A11 * b.A12 + a.A12 * b.A22,
                a.A21 * b.A11 + a.A22 * b.A21, a.A21 * b.A12 + a.A22 * b.A22);
        }

        public static Matrix2 operator *(Matrix2 a, double s)
        {
            return new Matrix2(
                a.A11 / s, a.A12 / s,
                a.A21 / s, a.A22 / s);
        }
    }
}
