using System;

namespace TriMesh
{
    public class Utility
    {
        /// <summary>
        /// Determines if the given floating point numbers can be considered equal.
        /// See: http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm
        /// </summary>
        /// <param name="a">first number.</param>
        /// <param name="b">second number.</param>
        public static bool AlmostEqual(double a, double b)
        {
            double maxRelativeError = 0.000001;
            double maxAbsoluteError = maxRelativeError * maxRelativeError;

            double absa = Math.Abs(a);
            double absb = Math.Abs(b);
            double diff = Math.Abs(a - b);

            if (diff < maxAbsoluteError)
                return true;

            double relativeError = (absb > absa ? diff / absb : diff / absa);
            if (relativeError <= maxRelativeError)
                return true;

            return false;
        }

        /// <summary>
        /// Determines if the given floating point number can be considered equal to zero.
        /// </summary>
        /// <param name="a">the number to check.</param>
        public static bool AlmostZero(double a)
        {
            return AlmostEqual(a, 0);
        }

    }
}
