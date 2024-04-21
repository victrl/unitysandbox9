using App.AppScenes;
using UnityEngine.SceneManagement;

namespace App.Core.Common
{
    public abstract class AppSceneMonoBehaviour : AppMonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
        
            if (AppContext.AppContext.IsInitialized == false)
            {
                SceneManager.LoadScene(AppScenes.AppScenes.AppContext.SceneName());
            }

            Init();
        }

        protected abstract void Init();
    }
}