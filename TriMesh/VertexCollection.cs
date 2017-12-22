using System;
using System.Collections;
using System.Collections.Generic;

namespace TriMesh
{
    public class VertexCollection : IReadOnlyCollection<Vertex>
    {
        private List<Vertex> vertices = new List<Vertex>();

        public int Count { get { return vertices.Count; } }

        internal void Add(Vertex v)
        {
            vertices.Add(v);
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
