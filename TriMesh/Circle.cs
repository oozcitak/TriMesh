using System;

namespace TriMesh
{
    public sealed class Circle
    {
        public Vertex Center { get; private set; }

        public double R { get; private set; }
        public double D { get; private set; }

        public double Area { get; private set; }
        public double Perimeter { get; private set; }

        public double X { get { return Center.X; } }
        public double Y { get { return Center.Y; } }

        public Circle(Vertex center, double r)
        {
            Center = center;
            R = r;
            D = 2 * r;
            Area = Math.PI * R * R;
            Perimeter = 2 * Math.PI * R;
        }

        public PointShapeRelation Contains(Vertex v)
        {
            double dist = (v - Center).Length2 - R;
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
