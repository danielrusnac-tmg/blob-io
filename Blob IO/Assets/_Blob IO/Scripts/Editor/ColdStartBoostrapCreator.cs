using UnityEditor;

namespace BlobIO
{
    public static class ColdStartBoostrapCreator
    {
        private const string BOOT_SCENE_PATH = "Assets/_Blob IO/Scenes/Boot.unity";
        private static ColdStartBoostrap s_coldStartBoostrap;

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            if (s_coldStartBoostrap != null)
                return;

            s_coldStartBoostrap = new ColdStartBoostrap(BOOT_SCENE_PATH);
        }
    }
}