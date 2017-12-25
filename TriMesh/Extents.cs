using System;

namespace TriMesh
{
    /// <summary>
    /// Represents 2D geometric extents.
    /// </summary>
    public class Extents
    {
        /// <summary>
        /// Gets the minimum X coordinate.
        /// </summary>
        public double Xmin { get; private set; }
        /// <summary>
        /// Gets the minimum Y coordinate.
        /// </summary>
        public double Ymin { get; private set; }
        /// <summary>
        /// Gets the maximum X coordinate.
        /// </summary>
        public double Xmax { get; private set; }
        /// <summary>
        /// Gets the maximum Y coordinate.
        /// </summary>
        public double Ymax { get; private set; }

        /// <summary>
        /// Gets the width of the extents.
        /// </summary>
        public double Width { get { return Xmax - Xmin; } }
        /// <summary>
        /// Gets the height of the extents.
        /// </summary>
        public double Height { get { return Ymax - Ymin; } }

        /// <summary>
        /// Instantiates a new extents object.
        /// </summary>
        public Extents()
        {
            Xmin = double.MaxValue;
            Ymin = double.MaxValue;
            Xmax = double.MinValue;
            Ymax = double.MinValue;
        }

        public Extents(double x, double y) : this()
        {
            Add(x, y);
        }

        /// <summary>
        /// Adds the given coordinate to the extents
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void Add(double x, double y)
        {
            Xmin = Math.Min(Xmin, x);
            Ymin = Math.Min(Ymin, y);
            Xmax = Math.Max(Xmax, x);
            Ymax = Math.Max(Ymax, y);
        }

        /// <summary>
        /// Returns a new extents by enlarging the current extents.
        /// </summary>
        /// <param name="dx">The amount to expand ix X direction</param>
        /// <param name="dy">The amount to expand ix Y direction</param>
        /// <returns>The expanded extents</returns>
        public Extents Expand(double dx, double dy)
        {
            Extents ex = new Extents();
            ex.Add(Xmin - dx, Ymin - dy);
            ex.Add(Xmax + dx, Ymax + dy);
            return ex;
        }

        public override string ToString()
        {
            return Xmin.ToString("F2") + " ~ " + Xmax.ToString("F2") + ", " + Ymin.ToString("F2") + " ~ " + Ymax.ToString("F2");
        }
    }
}
