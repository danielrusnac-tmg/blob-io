using UnityEditor.SceneManagement;
using UnityEngine;

namespace BlobIOEditor.LevelEditor
{
    public class LevelEditorStage : PreviewSceneStage
    {
        protected override GUIContent CreateHeaderContent()
        {
            return new GUIContent("Level Editor");
        }
    }
}