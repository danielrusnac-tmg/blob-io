using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    public class ChildSpringSetter : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float _damping;
        [SerializeField] private float _frequency;

        private void Reset()
        {
            UpdateSprings();
        }

        private void OnValidate()
        {
            UpdateSprings();
        }

        private void UpdateSprings()
        {
            SpringJoint2D[] springs = GetComponentsInChildren<SpringJoint2D>();

            foreach (SpringJoint2D spring in springs)
            {
                spring.dampingRatio = _damping;
                spring.frequency = _frequency;

#if UNITY_EDITOR
                EditorUtility.SetDirty(spring);
#endif
            }
        }
    }
}