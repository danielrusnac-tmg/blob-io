using System.Collections.Generic;
using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 10f;
        [SerializeField] private BodySpringSetting _surfaceSpring;
        [SerializeField] private BodySpringSetting _bodySpring;
        [SerializeField] private BetterBlobPoint _pointPrefab;

        private IControllableInput _input;
        private BetterBlobPoint[] _points;

        private void Awake()
        {
            CreatePoints();
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
        
        private int GetPointCount()
        {
            return (int)(2 * Mathf.PI * _radius / (1 / _resolution));
        }
        
        private void CreatePoints()
        {
            int count = GetPointCount();
            _points = new BetterBlobPoint[count];
            float angleStep = 360f / count;
            
            for (int i = 0; i < count; i++)
            {
                Vector3 position = transform.position + Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector3.right * _radius;
                _points[i] = Instantiate(_pointPrefab, position, Quaternion.identity, transform);
            }

            int minConnectionOffset = count / 2;
            for (int i = 0; i < count; i++)
            {
                _points[i].Connect(_points[GetPointIndex(i - 1, count)], _surfaceSpring);
                _points[i].Connect(_points[GetPointIndex(i + 1, count)], _surfaceSpring);
                _points[i].Connect(_points[GetPointIndex(i + minConnectionOffset, count)], _bodySpring);
            }
        }

        private int GetPointIndex(int index, int count)
        {
            return (int) Mathf.Repeat(index, count);
        } 
    }
}