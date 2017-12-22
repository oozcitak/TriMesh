using System;

namespace TriMesh
{
    public sealed class Matrix3
    {
        private double? determinant = null;

        public double A11 { get; private set; }
        public double A12 { get; private set; }
        public double A13 { get; private set; }
        public double A21 { get; private set; }
        public double A22 { get; private set; }
        public double A23 { get; private set; }
        public double A31 { get; private set; }
        public double A32 { get; private set; }
        public double A33 { get; private set; }

        public static Matrix3 Zero { get { return new Matrix3(0, 0, 0, 0, 0, 0, 0, 0, 0); } }
        public static Matrix3 Identity { get { return new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1); } }

        public double Determinant
        {
            get
            {
                if (determinant == null)
                {
                    determinant = A11 * (A22 * A33 - A32 * A23) - A12 * (A21 * A33 - A31 * A23) + A13 * (A21 * A32 - A31 * A22);
                }
                return determinant.Value;
            }
        }

        public Matrix3(double a11, double a12, double a13, double a21, double a22, double a23, double a31, double a32, double a33)
        {
            A11 = a11; A12 = a12; A13 = a13;
            A21 = a21; A22 = a22; A23 = a23;
            A31 = a31; A32 = a32; A33 = a33;
        }

        public static Matrix3 operator +(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(
                a.A11 + b.A11, a.A12 + b.A12, a.A13 + b.A13,
                a.A21 + b.A21, a.A22 + b.A22, a.A23 + b.A23,
                a.A31 + b.A31, a.A32 + b.A32, a.A33 + b.A33);
        }

        public static Matrix3 operator -(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(
                a.A11 - b.A11, a.A12 - b.A12, a.A13 - b.A13,
                a.A21 - b.A21, a.A22 - b.A22, a.A23 - b.A23,
                a.A31 - b.A31, a.A32 - b.A32, a.A33 - b.A33);
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(
                a.A11 * b.A11 + a.A12 * b.A21 + a.A13 * b.A31, a.A11 * b.A12 + a.A12 * b.A22 + a.A13 * b.A32, a.A11 * b.A13 + a.A12 * b.A23 + a.A13 * b.A33,
                a.A21 * b.A11 + a.A22 * b.A21 + a.A23 * b.A31, a.A21 * b.A12 + a.A22 * b.A22 + a.A23 * b.A32, a.A21 * b.A13 + a.A22 * b.A23 + a.A23 * b.A33,
                a.A31 * b.A11 + a.A32 * b.A21 + a.A33 * b.A31, a.A31 * b.A12 + a.A32 * b.A22 + a.A33 * b.A32, a.A31 * b.A13 + a.A32 * b.A23 + a.A33 * b.A33);
        }

        public static Matrix3 operator *(Matrix3 a, double s)
        {
            return new Matrix3(
                a.A11 / s, a.A12 / s, a.A13 / s,
                a.A21 / s, a.A22 / s, a.A23 / s,
                a.A31 / s, a.A32 / s, a.A33 / s);
        }
    }
}
