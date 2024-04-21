
using UnityEditor;
using UnityEditor.SceneManagement;

namespace App.Common.Tools
{
    public class OpenSceneMenu : Editor
    {
        [MenuItem("App/Play with AppContext %^&P", false, 0)]
        private static void PlayWithAppContext()
        {
            OpenSceneByPath("Assets/App/AppScenes/AppContext.unity");
        
            EditorApplication.EnterPlaymode();
        }

        [MenuItem("App/Go to Scene/AppContext %^&C", false, 1)]
        private static void AppContext()
        {
            OpenSceneByPath("Assets/App/AppScenes/AppContext.unity");
        }

        [MenuItem("App/Go to Scene/AppMenu %^&M", false, 2)]
        private static void AppMenu()
        {
            OpenSceneByPath("Assets/App/AppScenes/AppMenu.unity");
        }

        [MenuItem("App/Go to Scene/Game01 %^&1", false, 10)]
        private static void GameScene01()
        {
            OpenSceneByPath("Assets/App/AppScenes/Games/Game01.unity");
        }

        private static void OpenSceneByPath(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}