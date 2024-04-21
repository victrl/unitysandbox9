
using System;
using App.Core.UI.SceneTransitionService;
using PopupSystem;
using UnityEngine;
using Zenject;

namespace App.Common.UI.Elements.Buttons
{
    [RequireComponent(typeof(DefaultButtonAnimator))]
    public class BackButton : ButtonView
    {
        private Action onClickBack;

        [Inject]
        private SceneTransitionService sceneTransitionService;

        public void SetOverrideOnClickBackAction(Action onClickBack)
        {
            this.onClickBack = onClickBack;
        }

        private void Start()
        {
            AddListener(OnClick);
        }

        private void OnClick(IButtonView view)
        {
            if (onClickBack != null)
            {
                onClickBack?.Invoke();
                return;
            }
            
            sceneTransitionService.ToBack();
        }
    }
}
