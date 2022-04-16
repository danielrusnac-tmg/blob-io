using UnityEditor.SceneManagement;
using UnityEngine;

namespace BlobIO.LevelEditor
{
    public class LevelEditorStage : PreviewSceneStage
    {
        protected override GUIContent CreateHeaderContent()
        {
            return new GUIContent("Level Editor");
        }
    }
}