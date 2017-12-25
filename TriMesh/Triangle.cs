using System;
using System.Collections.Generic;

namespace TriMesh
{
    /// <summary>
    /// Represents a 2D triangle.
    /// </summary>
    public sealed class Triangle
    {
        private Vertex centroid = null;
        private Circle circumcircle = null;
        private double? signedArea = null;
        private double? minAngle = null;

        /// <summary>
        /// Gets or sets a user defined object associated with this instance.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the first vertex.
        /// </summary>
        public Vertex V1 { get; private set; }
        /// <summary>
        /// Gets the second vertex.
        /// </summary>
        public Vertex V2 { get; private set; }
        /// <summary>
        /// Gets the third vertex.
        /// </summary>
        public Vertex V3 { get; private set; }

        /// <summary>
        /// Gets the edge between vertices 1 and 2.
        /// </summary>
        public Halfedge S12 { get; private set; }
        /// <summary>
        /// Gets the edge between vertices 2 and 3.
        /// </summary>
        public Halfedge S23 { get; private set; }
        /// <summary>
        /// Gets the edge between vertices 3 and 1.
        /// </summary>
        public Halfedge S31 { get; private set; }

        /// <summary>
        /// Gets the triangle opposite the 1-2 edge.
        /// </summary>
        public Triangle N12 { get { return (S12 == null ? null : (S12.Opposite == null ? null : S12.Opposite.Parent)); } }
        /// <summary>
        /// Gets the triangle opposite the 2-3 edge.
        /// </summary>
        public Triangle N23 { get { return (S23 == null ? null : (S23.Opposite == null ? null : S23.Opposite.Parent)); } }
        /// <summary>
        /// Gets the triangle opposite the 3-1 edge.
        /// </summary>
        public Triangle N31 { get { return (S31 == null ? null : (S31.Opposite == null ? null : S31.Opposite.Parent)); } }

        internal bool removed = false;
        internal int mark = 0;

        /// <summary>
        /// Gets the centroid.
        /// </summary>
        public Vertex Centroid
        {
            get
            {
                if (centroid == null)
                {
                    centroid = Vertex.Average(V1, V2, V3);
                }
                return centroid;
            }
        }

        /// <summary>
        /// Gets the circumscribed circle.
        /// </summary>
        public Circle Circumcircle
        {
            get
            {
                if (circumcircle == null)
                {
                    circumcircle = new Circle(new Vertex(0, 0, 0), 0);
                    double a = SignedArea * 2;
                    if (!Utility.AlmostZero(a))
                    {
                        double bx = -1 * new Matrix3(
                            V1.X * V1.X + V1.Y * V1.Y, V1.Y, 1,
                            V2.X * V2.X + V2.Y * V2.Y, V2.Y, 1,
                            V3.X * V3.X + V3.Y * V3.Y, V3.Y, 1).Determinant;
                        double by = new Matrix3(
                            V1.X * V1.X + V1.Y * V1.Y, V1.X, 1,
                            V2.X * V2.X + V2.Y * V2.Y, V2.X, 1,
                            V3.X * V3.X + V3.Y * V3.Y, V3.X, 1).Determinant;
                        double c = -1 * new Matrix3(
                            V1.X * V1.X + V1.Y * V1.Y, V1.X, V1.Y,
                            V2.X * V2.X + V2.Y * V2.Y, V2.X, V2.Y,
                            V3.X * V3.X + V3.Y * V3.Y, V3.X, V3.Y).Determinant;
                        circumcircle = new Circle(new Vertex(-bx / (2 * a), -by / (2 * a), 0), Math.Sqrt((bx * bx + by * by - 4 * a * c)) / (2 * Math.Abs(a)));
                    }
                }
                return circumcircle;
            }
        }

        /// <summary>
        /// Gets the signed area.
        /// </summary>
        public double SignedArea
        {
            get
            {
                if (signedArea == null)
                {
                    double a = new Matrix3(
                        V1.X, V1.Y, 1,
                        V2.X, V2.Y, 1,
                        V3.X, V3.Y, 1).Determinant;
                    signedArea = a / 2;
                }
                return signedArea.Value;
            }
        }

        /// <summary>
        /// Gets the absolute area.
        /// </summary>
        public double Area { get { return Math.Abs(SignedArea); } }

        /// <summary>
        /// Gets the minimum ıf the corner angles.
        /// </summary>
        public double MinAngle
        {
            get
            {
                if (minAngle == null)
                {
                    double d = Math.Min(S12.Length, Math.Min(S23.Length, S31.Length));
                    minAngle = Math.Asin(d / (2 * Circumcircle.R));
                }
                return minAngle.Value;
            }
        }

        /// <summary>
        /// Gets whether any of the corner vertices are super triangle vertices.
        /// </summary>
        public bool IsSuperTriangle { get { return V1.IsSuperVertex | V2.IsSuperVertex | V3.IsSuperVertex; } }

        /// <summary>
        /// Instantiates a new triangle object.
        /// </summary>
        /// <param name="v1">The first vertex</param>
        /// <param name="v2">The second vertex</param>
        /// <param name="v3">The third vertex</param>
        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;

            S12 = new Halfedge(v1, v2);
            S23 = new Halfedge(v2, v3);
            S31 = new Halfedge(v3, v1);

            S12.SetMeshParams(this, S31, S23, null);
            S23.SetMeshParams(this, S12, S31, null);
            S31.SetMeshParams(this, S23, S12, null);
        }

        /// <summary>
        /// Determines whether the triangle contains the given vertex.
        /// </summary>
        /// <param name="v">The vertex to check</param>
        /// <returns>The relative location of the vertex</returns>
        public PointShapeRelation Contains(Vertex v)
        {
            double a12 = new Matrix3(
                V1.X, V1.Y, 1,
                V2.X, V2.Y, 1,
                v.X, v.Y, 1).Determinant;
            double a23 = new Matrix3(
                V2.X, V2.Y, 1,
                V3.X, V3.Y, 1,
                v.X, v.Y, 1).Determinant;
            double a31 = new Matrix3(
                V3.X, V3.Y, 1,
                V1.X, V1.Y, 1,
                v.X, v.Y, 1).Determinant;
            if (Utility.AlmostZero(a12) || Utility.AlmostZero(a23) || Utility.AlmostZero(a31))
            {
                return PointShapeRelation.On;
            }
            else if (a12 < 0 || a23 < 0 || a31 < 0)
            {
                return PointShapeRelation.Outside;
            }
            else
            {
                return PointShapeRelation.Inside;
            }
        }

        /// <summary>
        /// Determines whether the triangle contains the given vertex.
        /// </summary>
        /// <param name="v">The vertex to check</param>
        /// <param name="location">On return, contains where on the triangle the vertex lies</param>
        /// <returns>The relative location of the vertex</returns>
        public PointShapeRelation Contains(Vertex v, out PointOnTriangle location)
        {
            double a12 = new Matrix3(
                V1.X, V1.Y, 1,
                V2.X, V2.Y, 1,
                v.X, v.Y, 1).Determinant;
            double a23 = new Matrix3(
                V2.X, V2.Y, 1,
                V3.X, V3.Y, 1,
                v.X, v.Y, 1).Determinant;
            double a31 = new Matrix3(
                V3.X, V3.Y, 1,
                V1.X, V1.Y, 1,
                v.X, v.Y, 1).Determinant;

            if (Utility.AlmostZero(a12))
            {
                if (Utility.AlmostZero(a23))
                    location = PointOnTriangle.OnV2;
                else if (Utility.AlmostZero(a31))
                    location = PointOnTriangle.OnV1;
                else
                    location = PointOnTriangle.OnS12;
                return PointShapeRelation.On;
            }
            else if (Utility.AlmostZero(a23))
            {
                if (Utility.AlmostZero(a31))
                    location = PointOnTriangle.OnV3;
                else if (Utility.AlmostZero(a12))
                    location = PointOnTriangle.OnV2;
                else
                    location = PointOnTriangle.OnS23;
                return PointShapeRelation.On;
            }
            else if (Utility.AlmostZero(a31))
            {
                if (Utility.AlmostZero(a12))
                    location = PointOnTriangle.OnV1;
                else if (Utility.AlmostZero(a23))
                    location = PointOnTriangle.OnV3;
                else
                    location = PointOnTriangle.OnS31;
                return PointShapeRelation.On;
            }
            else if (a12 < 0 || a23 < 0 || a31 < 0)
            {
                location = PointOnTriangle.None;
                return PointShapeRelation.Outside;
            }
            else
            {
                location = PointOnTriangle.None;
                return PointShapeRelation.Inside;
            }
        }

        /// <summary>
        /// Determines whether the triangle contains the given vertex.
        /// </summary>
        /// <param name="v">The vertex to check</param>
        /// <param name="location">On return, contains where on the triangle the vertex lies</param>
        /// <param name="closestEdge">On return, contains the edge closest to the input vertex</param>
        /// <returns>The relative location of the vertex</returns>
        public PointShapeRelation Contains(Vertex v, out PointOnTriangle location, out Halfedge closestEdge)
        {
            double a12 = new Matrix3(
                V1.X, V1.Y, 1,
                V2.X, V2.Y, 1,
                v.X, v.Y, 1).Determinant;
            double a23 = new Matrix3(
                V2.X, V2.Y, 1,
                V3.X, V3.Y, 1,
                v.X, v.Y, 1).Determinant;
            double a31 = new Matrix3(
                V3.X, V3.Y, 1,
                V1.X, V1.Y, 1,
                v.X, v.Y, 1).Determinant;

            if (Utility.AlmostZero(a12))
            {
                if (Utility.AlmostZero(a23))
                    location = PointOnTriangle.OnV2;
                else if (Utility.AlmostZero(a31))
                    location = PointOnTriangle.OnV1;
                else
                    location = PointOnTriangle.OnS12;
                closestEdge = S12;
                return PointShapeRelation.On;
            }
            else if (Utility.AlmostZero(a23))
            {
                if (Utility.AlmostZero(a31))
                    location = PointOnTriangle.OnV3;
                else if (Utility.AlmostZero(a12))
                    location = PointOnTriangle.OnV2;
                else
                    location = PointOnTriangle.OnS23;
                closestEdge = S23;
                return PointShapeRelation.On;
            }
            else if (Utility.AlmostZero(a31))
            {
                if (Utility.AlmostZero(a12))
                    location = PointOnTriangle.OnV1;
                else if (Utility.AlmostZero(a23))
                    location = PointOnTriangle.OnV3;
                else
                    location = PointOnTriangle.OnS31;
                closestEdge = S31;
                return PointShapeRelation.On;
            }
            else if (a12 < 0 || a23 < 0 || a31 < 0)
            {
                if (a12 < 0)
                    closestEdge = S12;
                else if (a23 < 0)
                    closestEdge = S23;
                else
                    closestEdge = S31;
                location = PointOnTriangle.None;
                return PointShapeRelation.Outside;
            }
            else
            {
                double d12 = a12 / S12.Length;
                double d23 = a23 / S23.Length;
                double d31 = a31 / S31.Length;
                if (d12 < d23 && d12 < d31)
                    closestEdge = S12;
                else if (d23 < d12 && d23 < d31)
                    closestEdge = S23;
                else
                    closestEdge = S31;

                location = PointOnTriangle.None;
                return PointShapeRelation.Inside;
            }
        }

        /// <summary>
        /// Sets the details about neighbouring edges in the mesh.
        /// </summary>
        /// <param name="s12opp">The edge opposite of edge 1-2</param>
        /// <param name="s23opp">The edge opposite of edge 2-3</param>
        /// <param name="s31opp">The edge opposite of edge 3-1</param>
        internal void SetMeshParams(Halfedge s12opp, Halfedge s23opp, Halfedge s31opp)
        {
            if (s12opp != null) { S12.Opposite = s12opp; s12opp.Opposite = S12; }
            if (s23opp != null) { S23.Opposite = s23opp; s23opp.Opposite = S23; }
            if (s31opp != null) { S31.Opposite = s31opp; s31opp.Opposite = S31; }
        }
    }
}
