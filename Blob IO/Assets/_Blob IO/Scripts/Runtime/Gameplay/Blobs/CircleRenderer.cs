using UnityEngine;

namespace BlobIO.Blobs
{
    public class CircleRenderer
    {
        private readonly int _count;
        private readonly Mesh _mesh;
        private Vector3 _center;
        private readonly Vector3[] _vertices;
        private readonly Vector3[] _normals;
        private readonly int[] _triangles;

        public CircleRenderer(MeshFilter meshFilter, int count)
        {
            _count = count;
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;
            
            _vertices = new Vector3[count + 1];
            _normals = new Vector3[count + 1];
            _triangles = new int[count * 3];
        }

        public void UpdateMesh(BlobPoint[] points)
        {
            CalculateMeshTriangulation(points);
            UpdateMeshData();
        }

        private void UpdateMeshData()
        {
            _mesh.SetVertices(_vertices);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.SetNormals(_normals);
            _mesh.RecalculateBounds();
        }

        private void CalculateMeshTriangulation(BlobPoint[] points)
        {
            _center = Vector3.zero;
            
            for (int i = 0, triangle = 0; i < _count; i++, triangle += 3)
            {
                _center += points[i].Position;
                _vertices[i] = points[i].Position;
                _normals[i] = points[i].Normal;
                CreateTriangle(triangle, i);
            }

            _center /= _count;
            _vertices[_count] = _center;
            _normals[_count] = -Vector3.forward;
        }

        private void CreateTriangle(int triangle, int vertex)
        {
            if (vertex < _count - 1)
            {
                _triangles[triangle + 0] = vertex + 1;
                _triangles[triangle + 1] = vertex;
                _triangles[triangle + 2] = _count;
            }
            else
            {
                _triangles[triangle + 0] = 0;
                _triangles[triangle + 1] = _count - 1;
                _triangles[triangle + 2] = _count;
            }
        }
    }
}