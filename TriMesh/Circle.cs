using System;

namespace TriMesh
{
    /// <summary>
    /// Represents a 2D circle.
    /// </summary>
    public sealed class Circle
    {
        /// <summary>
        /// Gets the center vertex of the circle.
        /// </summary>
        public Vertex Center { get; private set; }

        /// <summary>
        /// Gets the radius of the circle.
        /// </summary>
        public double R { get; private set; }
        /// <summary>
        /// Gets the diameter of the circle.
        /// </summary>
        public double D { get; private set; }

        /// <summary>
        /// Gets the area of the circle.
        /// </summary>
        public double Area { get; private set; }
        /// <summary>
        /// Gets the perimeter of the circle.
        /// </summary>
        public double Perimeter { get; private set; }

        /// <summary>
        /// Gets the X coordinate of the center vertex.
        /// </summary>
        public double X { get { return Center.X; } }
        /// <summary>
        /// Gets the Y coordinate of the center vertex.
        /// </summary>
        public double Y { get { return Center.Y; } }

        /// <summary>
        /// Instantiates a new circle centered on the given vertex and having
        /// the given radius.
        /// </summary>
        /// <param name="center">The center vertex</param>
        /// <param name="r">The radius of the circle</param>
        public Circle(Vertex center, double r)
        {
            Center = center;
            R = r;
            D = 2 * r;
            Area = Math.PI * R * R;
            Perimeter = 2 * Math.PI * R;
        }

        /// <summary>
        /// Determines whether the circle contains the given vertex.
        /// </summary>
        /// <param name="v">The vertex to check</param>
        /// <returns>The relative location of the vertex</returns>
        public PointShapeRelation Contains(Vertex v)
        {
            double dist = (v - Center).Length - R;
            if (Utility.AlmostZero(dist))
                return PointShapeRelation.On;
            else if (dist < 0)
                return PointShapeRelation.Inside;
            else
                return PointShapeRelation.Outside;
        }

        public override string ToString()
        {
            return Center.ToString() + ", R = " + R.ToString("F2");
        }
    }
}
