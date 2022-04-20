using UnityEngine;

namespace BlobIO.SoftBody
{
    public class SoftBodyRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;

        private int _count;
        private Vector3 _center;
        private Mesh _mesh;
        
        private int[] _triangles;
        private Vector3[] _vertices;
        private Vector3[] _normals;

        public void CreateMesh(SoftBodyPoint[] points)
        {
            _count = points.Length;
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
            
            _vertices = new Vector3[_count + 1];
            _normals = new Vector3[_count + 1];
            _triangles = new int[_count * 3];
        }

        public void UpdateMesh(SoftBodyPoint[] points, SoftBodySpring[] springs)
        {
            CalculateMeshTriangulation(points, springs);
            
            _mesh.SetVertices(_vertices);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.SetNormals(_normals);
            _mesh.RecalculateBounds();
        }
        
        private void CalculateMeshTriangulation(SoftBodyPoint[] points, SoftBodySpring[] springs)
        {
            _center = Vector3.zero;
            
            for (int i = 0, triangle = 0; i < _count; i++, triangle += 3)
            {
                Vector3 position = _meshFilter.transform.InverseTransformPoint(points[i].Position);
                Vector3 normal = _meshFilter.transform.InverseTransformDirection(springs[i].Normal);
                
                _center += position;
                _vertices[i] = position;
                _normals[i] = normal;
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