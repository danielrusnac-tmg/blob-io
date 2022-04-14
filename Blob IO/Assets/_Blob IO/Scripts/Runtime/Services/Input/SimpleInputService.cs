using UnityEngine;

namespace BlobIO.Services.Input
{
    public class SimpleInputService : IInputService
    {
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";

        public Vector2 Axis => new Vector2(SimpleInput.GetAxisRaw(HORIZONTAL), SimpleInput.GetAxisRaw(VERTICAL));
    }
}