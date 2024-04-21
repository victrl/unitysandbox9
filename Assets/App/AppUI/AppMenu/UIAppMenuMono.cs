using App.AppScenes;
using App.Core.Common;
using App.Core.UI;
using App.Core.UI.PopupService;
using App.Core.UI.PopupService.Popups;
using App.Core.UI.SceneTransitionService;
using UnityEngine.UIElements;
using Zenject;

namespace App.AppUI.AppMenu
{
    public class UIAppMenuMono : AppMonoBehaviour
    {
        private Button playButton;

        [Inject]
        private SceneTransitionService sceneTransitionService;

        [Inject]
        private UIService uiService;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            playButton = root.Q<Button>("PlayButton");
            playButton.clicked += OnClickPlayButton;
        }

        private void OnClickPlayButton()
        {
            uiService.ShowPopup(PopupNames.Test, new TestModel()
            {
                Title = "Test",
                OnClickOk = (_) =>
                {
                    sceneTransitionService.OpenScene(AppScenes.AppScenes.Game01.SceneName());
                    uiService.ClosePopup();
                }
            });
        }
    }
}