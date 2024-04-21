
using System;
using System.Collections.Generic;
using App.Core.Common;
using PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = App.Common.Tools.Logger;

namespace App.Common.UI.Elements.Buttons
{
    public class ButtonView : AppMonoBehaviour, IButtonView
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image view;

        public Button Button => button;
        public TMP_Text Text => text;

        private IButtonOverrideComponent overrideComponent;
        private IButtonAnimator buttonAnimator;

        private readonly List<Action<IButtonView>> listeners = new List<Action<IButtonView>>();

        protected override void Awake()
        {
            base.Awake();

            InitComponents();
            SetDefaultText();
        }

        public void AddListener(Action<IButtonView> onClick)
        {
            if (onClick == null)
            {
                return;
            }

            listeners.Add(onClick);
        }

        public void RemoveAllListeners()
        {
            listeners.Clear();
        }

        private void OnClick()
        {
            buttonAnimator?.OnClickAnimation(view);

            for (var i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                try
                {
                    listener?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }

        private void InitComponents()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            button.onClick.AddListener(OnClick);

            if (view == null)
            {
                view = GetComponent<Image>();
            }

            overrideComponent = GetComponentInChildren<IButtonOverrideComponent>();
            buttonAnimator = GetComponentInChildren<IButtonAnimator>();
            SetButtonTransition();
        }

        private void SetButtonTransition()
        {
            button.transition = buttonAnimator != null ? Selectable.Transition.None : Selectable.Transition.SpriteSwap;
        }

        public T GetAdditionalData<T>() where T : class
        {
            return overrideComponent?.GetAdditionalData<T>();
        }

        public void SetButtonState(bool state)
        {
            button.interactable = state;
        }

        private void SetDefaultText()
        {
            if (text == null || overrideComponent == null) return;

            var defaultText = overrideComponent.GetDefaultText();
            if (string.IsNullOrEmpty(defaultText) == false)
            {
                text.text = overrideComponent.GetDefaultText();
            }
        }
    }
}