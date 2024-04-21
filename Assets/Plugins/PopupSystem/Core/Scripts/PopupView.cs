using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PopupSystem.Animators;
using UnityEngine;

namespace PopupSystem
{
    public class PopupView : MonoBehaviour
    {
        [Header("Animatinons parameters")] 
        [SerializeField] private DefaultAnimatorContainer AnimatorContainer;

        protected PopupService popupService;
        protected bool IsCanCloseOnButton = true;
        private IPopupModel model;

        private List<IButtonView> buttons = new List<IButtonView>();

        public virtual void Init(PopupService service, IPopupModel model)
        {
            popupService = service;
            SetModel(model);
            InitialComponent();
        }

        protected virtual void SetModel(IPopupModel model)
        {
            if (model != null)
            {
                this.model = model;
            }
            else
            {
                string error = $"[PopupView] => SetModel: Model isn't null";
                throw new Exception(error);
            }
        }

        public async void Show(Action onComplete = null)
        {
            OnViewShow();
            AnimatorContainer.Animator.Play("Open");
            await InvokeDelayedCall(AnimatorContainer.OpenWindowClip.length, () =>
            {
                OnViewShowComplete();
                onComplete?.Invoke();
            });
        }

        public async virtual void Hide(Action onComplete = null)
        {
            OnViewClose();
            AnimatorContainer.Animator.Play("Close");
            await InvokeDelayedCall(AnimatorContainer.CloseWindowClip.length, () =>
            {
                OnViewCloseComplete();
                onComplete?.Invoke();
            });
        }

        protected void DisableCloseButton()
        {
            IsCanCloseOnButton = false;
            foreach (var child in transform.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "#CloseButton")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        protected virtual void OnViewShow()
        {
            Debug.Log("OnViewShow");
        }
        
        protected virtual void OnViewShowComplete()
        {
            Debug.Log("OnViewShow");
        }

        protected virtual void OnViewClose()
        {
            DeinitButtons();
            Debug.Log("OnViewClose");
        }

        protected virtual void OnViewCloseComplete()
        {
            Debug.Log("OnViewCloseComplete");
        }

        protected void InitButton(IButtonView buttonView, Action<IButtonView> onClick)
        {
            if (buttonView == null)
            {
                return;
            }

            buttonView.RemoveAllListeners();
            buttonView.AddListener(onClick);
            buttons.Add(buttonView);
        }

        private void DeinitButtons()
        {
            foreach (var button in buttons)
            {
                button.RemoveAllListeners();
            }

            buttons.Clear();
        }

        public void OnClickClose(IButtonView buttonView)
        {
            if (IsCanCloseOnButton == false)
            {
                return;
            }

            popupService.ClosePopup();
        }

        private async Task InvokeDelayedCall(float delay, Action action)
        {
            int millisecondDelay = (int) (delay * 1000);
            await Task.Delay(millisecondDelay);
            action?.Invoke();
        }

        private void InitialComponent()
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>(true))
            {
                SetOnClickClose(child);
            }
        }

        private void SetOnClickClose(Transform child)
        {
            if (OnClickCloseComponentName.Contains(child.name))
            {
                IButtonView btn = child.GetComponent<IButtonView>();
                InitButton(btn, OnClickClose);
            }
        }

        private readonly List<string> OnClickCloseComponentName = new List<string>() {"#BackOnClick", "#CloseButton"};
    }

    public abstract class PopupView<TModel> : PopupView where TModel : class
    {
        protected TModel Model { get; private set; }

        protected override void SetModel(IPopupModel model)
        {
            base.SetModel(model);
            if (model is TModel resultModel)
            {
                Model = resultModel;
            }
            else
            {
                string error = $"[PopupView] => SetModel: Model isn't {typeof(TModel)}";
                throw new Exception(error);
            }
        }
    }
}