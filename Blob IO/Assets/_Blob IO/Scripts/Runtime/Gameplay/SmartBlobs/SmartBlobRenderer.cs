using UnityEngine;

namespace BlobIO.SmartBlobs
{
    public class SmartBlobRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Transform[] _tentacleBases;

        private int _vertexCount;
        private int _trianglesCount;
        private Vector3[] _vertices;
        private int[] _triangles;
        private Mesh _mesh;

        private void Awake()
        {
            _vertexCount = _tentacleBases.Length + 2;
            _trianglesCount = _tentacleBases.Length * 3;

            _vertices = new Vector3[_vertexCount];
            _triangles = new int[_trianglesCount];
            
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
        }

        private void Update()
        {
            float angle = 0f;
            float segmentWidth = Mathf.PI * 2f / _tentacleBases.Length;
            
            Vector2 center = Vector2.zero;
            foreach (Transform t in _tentacleBases)
                center += (Vector2)t.position;
            center /= _tentacleBases.Length;

            _vertices[0] = center;
            
            for (int i = 1; i < _vertexCount; ++i)
            {
                _vertices[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f); 
                angle += segmentWidth;
                
                if (i > 1)
                {
                    int j = (i - 2) * 3;
                    _triangles[j + 0] = 0;
                    _triangles[j + 1] = i - 1;
                    _triangles[j + 2] = i;
                }
            }
            
            _mesh.SetVertices(_vertices);
            _mesh.SetIndices(_triangles, MeshTopology.Triangles, 0);
            _mesh.RecalculateBounds();
        }
    }
}