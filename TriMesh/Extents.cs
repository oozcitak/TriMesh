using System;

namespace TriMesh
{
    public class Extents
    {
        public double Xmin { get; private set; }
        public double Ymin { get; private set; }
        public double Xmax { get; private set; }
        public double Ymax { get; private set; }

        public double Width { get { return Xmax - Xmin; } }
        public double Height { get { return Ymax - Ymin; } }

        public Extents()
        {
            Reset();
        }

        public Extents(double x, double y) : this()
        {
            Add(x, y);
        }

        public void Add(double x, double y)
        {
            Xmin = Math.Min(Xmin, x);
            Ymin = Math.Min(Ymin, y);
            Xmax = Math.Max(Xmax, x);
            Ymax = Math.Max(Ymax, y);
        }

        public void Reset()
        {
            Xmin = double.MaxValue;
            Ymin = double.MaxValue;
            Xmax = double.MinValue;
            Ymax = double.MinValue;
        }

        public override string ToString()
        {
            return Xmin.ToString("F2") + " ~ " + Xmax.ToString("F2") + ", " + Ymin.ToString("F2") + " ~ " + Ymax.ToString("F2");
        }

        public Extents Offset(double dx,double dy)
        {
            Extents ex = new Extents();
            ex.Add(Xmin - dx, Ymin - dy);
            ex.Add(Xmax + dx, Ymax + dy);
            return ex;
        }
    }
}
