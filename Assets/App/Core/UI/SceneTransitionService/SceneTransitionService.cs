using System.Collections.Generic;
using App.Common.Tools;
using App.Core.Common.Services;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace App.Core.UI.SceneTransitionService
{
    public class SceneTransitionService : AppService
    {
        private SceneTransitionAnimation sceneTransitionAnimation;
        private Dictionary<string, object> transitionData = new Dictionary<string, object>();
        private Stack<string> sceneHistory = new Stack<string>();

        public override void OnRegister()
        {
            sceneTransitionAnimation = Context.SceneTransitionAnimation;
            base.OnRegister();
        }

        public void OpenScene(string sceneName, object data = null)
        {
            if (data != null)
            {
                transitionData[sceneName] = data;
            }

            DOTween.Sequence()
                .Append(sceneTransitionAnimation.HideScene())
                .AppendCallback(() =>
                {
                    sceneHistory.Push(SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene(sceneName);
                })
                .Append(sceneTransitionAnimation.ShowScene());
        }

        public void ToBack()
        {
            var sceneName = sceneHistory.Pop();

            if (string.IsNullOrEmpty(sceneName))
            {
                Logger.LogWarning($"[SceneTransitionService] => Back: This scene is first");
                return;
            }

            DOTween.Sequence()
                .Append(sceneTransitionAnimation.HideScene())
                .AppendCallback(() =>
                {
                    SceneManager.LoadScene(sceneName);
                })
                .Append(sceneTransitionAnimation.ShowScene());
        }
    }
}