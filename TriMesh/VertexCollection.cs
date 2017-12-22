using System;
using System.Collections;
using System.Collections.Generic;

namespace TriMesh
{
    /// <summary>
    /// Represents a collection of vertices.
    /// </summary>
    public class VertexCollection : IReadOnlyCollection<Vertex>
    {
        private List<Vertex> vertices = new List<Vertex>();

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int Count { get { return vertices.Count; } }

        /// <summary>
        /// Adds a new vertex to the collection.
        /// </summary>
        /// <param name="v">New vertex</param>
        internal void Add(Vertex v)
        {
            vertices.Add(v);
        }

        /// <summary>
        /// Enumerates all vertices.
        /// of the mesh.
        /// </summary>
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
