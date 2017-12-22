using System;

namespace TriMesh
{
    public enum PointShapeRelation
    {
        Inside = -1,
        On = 0,
        Outside = 1
    }

    public enum PointOnTriangle
    {
        None,
        OnV1,
        OnV2,
        OnV3,
        OnS12,
        OnS23,
        OnS31
    }
}
