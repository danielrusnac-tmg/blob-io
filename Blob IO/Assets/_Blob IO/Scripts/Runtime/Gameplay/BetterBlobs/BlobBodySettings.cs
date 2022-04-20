using System;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    [Serializable]
    public class BlobBodySettings
    {
        [SerializeField] private LayerMask _solidMask;
        [SerializeField] private float _floorY;
        [SerializeField] private float _pressure;
        [SerializeField] private float _stiffness;
        [SerializeField] private float _damp;

        public LayerMask SolidMask => _solidMask;
        public float FloorY => _floorY;
        public float Pressure => _pressure;
        public float Stiffness => _stiffness;
        public float Damp => _damp;
    }
}