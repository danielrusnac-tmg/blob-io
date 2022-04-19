using System;

namespace BlobIOEditor.ColdStart
{
    [Serializable]
    public struct ColdStartData
    {
        public string[] LastOpenedScenesPaths;

        public ColdStartData(string[] lastOpenedScenesPaths)
        {
            LastOpenedScenesPaths = lastOpenedScenesPaths;
        }
    }
}