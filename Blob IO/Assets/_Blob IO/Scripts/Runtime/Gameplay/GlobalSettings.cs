﻿using UnityEngine;

namespace BlobIO.Gameplay
{
    [CreateAssetMenu(fileName = "New Global Settings", menuName = CreationPaths.CREATE + "Global Settings")]
    public class GlobalSettings : ScriptableObject
    {
        [SerializeField] private LayerMask _wallMask;

        public LayerMask WallMask => _wallMask;
    }
}