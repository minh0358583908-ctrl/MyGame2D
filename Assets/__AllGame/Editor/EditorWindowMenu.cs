using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorWindowMenu
{
    [MenuItem("Scenes/1.PixelAdventure", false, 101)]
    public static void OpenScenePixelAdventure()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/_PixelAdventure/Scenes/GameScene.unity");
    }
    [MenuItem("Scenes/2.Runner", false, 102)]
    public static void OpenSceneRunner()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/_Runner/Scenes/Runner_GameScene.unity");
    }
}