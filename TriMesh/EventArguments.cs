using System;
using System.Collections.Generic;

namespace TriMesh
{
    public class InsertVertexEventArgs : EventArgs
    {
        public Vertex NewVertex { get; private set; }
        public IEnumerable<Triangle> CurrentMesh { get; private set; }

        public InsertVertexEventArgs(Vertex vertex, IEnumerable<Triangle> mesh)
        {
            NewVertex = vertex;
            CurrentMesh = mesh;
        }
    }

    public class DividingTriangleEventArgs : EventArgs
    {
        public Vertex NewVertex { get; private set; }
        public IEnumerable<Triangle> CurrentMesh { get; private set; }
        public IEnumerable<Triangle> Triangles { get; private set; }

        public DividingTriangleEventArgs(Vertex vertex, IEnumerable<Triangle> mesh, IEnumerable<Triangle> tri)
        {
            NewVertex = vertex;
            CurrentMesh = mesh;
            Triangles = tri;
        }
    }

    public class DividedTriangleEventArgs : EventArgs
    {
        public Vertex NewVertex { get; private set; }
        public IEnumerable<Triangle> CurrentMesh { get; private set; }
        public IEnumerable<Triangle> DividedTriangles { get; private set; }
        public IEnumerable<Triangle> NewTriangles { get; private set; }

        public DividedTriangleEventArgs(Vertex vertex, IEnumerable<Triangle> mesh, IEnumerable<Triangle> tri, IEnumerable<Triangle> add)
        {
            NewVertex = vertex;
            CurrentMesh = mesh;
            DividedTriangles = tri;
            NewTriangles = add;
        }
    }

    public class FlippingEdgeEventArgs : EventArgs
    {
        public Halfedge Edge { get; private set; }
        public IEnumerable<Triangle> CurrentMesh { get; private set; }
        public Triangle BadTriangle1 { get; private set; }
        public Triangle BadTriangle2 { get; private set; }

        public FlippingEdgeEventArgs(Halfedge edge, IEnumerable<Triangle> mesh, Triangle t1, Triangle t2)
        {
            Edge = edge;
            CurrentMesh = mesh;
            BadTriangle1 = t1;
            BadTriangle2 = t2;
        }
    }

    public class FlippedEdgeEventArgs : EventArgs
    {
        public Halfedge Edge { get; private set; }
        public IEnumerable<Triangle> CurrentMesh { get; private set; }
        public Triangle NewTriangle1 { get; private set; }
        public Triangle NewTriangle2 { get; private set; }

        public FlippedEdgeEventArgs(Halfedge edge, IEnumerable<Triangle> mesh, Triangle t1, Triangle t2)
        {
            Edge = edge;
            CurrentMesh = mesh;
            NewTriangle1 = t1;
            NewTriangle2 = t2;
        }
    }

    public delegate void InsertVertexEventHandler(object sender, InsertVertexEventArgs e);
    public delegate void DividingTriangleEventHandler(object sender, DividingTriangleEventArgs e);
    public delegate void DividedTriangleEventHandler(object sender, DividedTriangleEventArgs e);
    public delegate void FlippingEdgeEventHandler(object sender, FlippingEdgeEventArgs e);
    public delegate void FlippedEdgeEventHandler(object sender, FlippedEdgeEventArgs e);
}
