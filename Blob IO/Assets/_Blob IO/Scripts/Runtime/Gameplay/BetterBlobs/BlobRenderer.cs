using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BlobRenderer
    {
        public readonly Vector3[] Vertices;
        public readonly int[] Triangles;
        
        private readonly int _count;
        private readonly Mesh _mesh;
        private Vector3 _center;
        private readonly Vector3[] _normals;


        public BlobRenderer(MeshFilter meshFilter, int count)
        {
            _count = count;
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;
            
            Vertices = new Vector3[count + 1];
            _normals = new Vector3[count + 1];
            Triangles = new int[count * 3];
        }

        public void UpdateMesh(BlobPoint[] points, Spring[] springs)
        {
            CalculateMeshTriangulation(points, springs);
            UpdateMeshData();
        }

        private void UpdateMeshData()
        {
            _mesh.SetVertices(Vertices);
            _mesh.SetTriangles(Triangles, 0);
            _mesh.SetNormals(_normals);
            _mesh.RecalculateBounds();
        }

        private void CalculateMeshTriangulation(BlobPoint[] points, Spring[] springs)
        {
            _center = Vector3.zero;
            
            for (int i = 0, triangle = 0; i < _count; i++, triangle += 3)
            {
                _center += points[i].Position;
                Vertices[i] = points[i].Position;
                _normals[i] = springs[i].Normal;
                CreateTriangle(triangle, i);
            }

            _center /= _count;
            Vertices[_count] = _center;
            _normals[_count] = -Vector3.forward;
        }

        private void CreateTriangle(int triangle, int vertex)
        {
            if (vertex < _count - 1)
            {
                Triangles[triangle + 0] = vertex + 1;
                Triangles[triangle + 1] = vertex;
                Triangles[triangle + 2] = _count;
            }
            else
            {
                Triangles[triangle + 0] = 0;
                Triangles[triangle + 1] = _count - 1;
                Triangles[triangle + 2] = _count;
            }
        }
    }
}