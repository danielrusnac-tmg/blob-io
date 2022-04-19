using System;

namespace BlobIOEditor.ColdStart
{
    [Serializable]
    public class ColdStartData
    {
        public bool WasColdStarted;
        public string[] LastOpenedScenesPaths;

        public ColdStartData()
        {
            LastOpenedScenesPaths = Array.Empty<string>();
        }
    }
}