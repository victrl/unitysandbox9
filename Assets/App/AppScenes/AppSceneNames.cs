
namespace App.AppScenes
{
    public static class AppSceneNames
    {
        public static string SceneName(this AppScenes scene)
        {
            return scene.ToString();
        }
    }

    public enum AppScenes
    {
        AppContext,
        AppMenu,
        Game01
    }
}
