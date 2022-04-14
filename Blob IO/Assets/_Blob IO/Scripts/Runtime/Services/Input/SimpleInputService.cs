using UnityEngine;

namespace BlobIO.Services.Input
{
    public class SimpleInputService : IInputService
    {
        protected const string HORIZONTAL = "Horizontal";
        protected const string VERTICAL = "Vertical";

        public Vector2 Axis => new Vector2(SimpleInput.GetAxis(HORIZONTAL), SimpleInput.GetAxis(VERTICAL));
    }
}