using System;

namespace TriMesh
{
    /// <summary>
    /// Defines the location of a vertex relative to a shape.
    /// </summary>
    public enum PointShapeRelation
    {
        /// <summary>
        /// The vertex lies inside the shape.
        /// </summary>
        Inside = -1,
        /// <summary>
        /// The vertex lies directly on the boundary of the shape.
        /// </summary>
        On = 0,
        /// <summary>
        /// The vertex lies outside the shape.
        /// </summary>
        Outside = 1
    }

    /// <summary>
    /// Defines the location of a vertex relative to the boundary of a triangle.
    /// </summary>
    public enum PointOnTriangle
    {
        /// <summary>
        /// The vertex is not on the boundary.
        /// </summary>
        None,
        /// <summary>
        /// The vertex lies directly on vertex 1.
        /// </summary>
        OnV1,
        /// <summary>
        /// The vertex lies directly on vertex 2.
        /// </summary>
        OnV2,
        /// <summary>
        /// The vertex lies directly on vertex 3.
        /// </summary>
        OnV3,
        /// <summary>
        /// The vertex lies on the edge connecting vertices 1 and 2.
        /// </summary>
        OnS12,
        /// <summary>
        /// The vertex lies on the edge connecting vertices 2 and 3.
        /// </summary>
        OnS23,
        /// <summary>
        /// The vertex lies on the edge connecting vertices 3 and 1.
        /// </summary>
        OnS31
    }
}
