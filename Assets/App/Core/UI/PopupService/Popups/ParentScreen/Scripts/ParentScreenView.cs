using App.Common.UI.Elements.Buttons;
using PopupSystem;
using TMPro;
using UnityEngine;

namespace App.Core.UI.PopupService.Popups
{
    public class ParentScreenView : PopupView<ParentScreenModel>
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private ButtonView acceptButtons;

        protected override void OnViewShow()
        {
            title.text = Model.Title;
            InitButton(acceptButtons, OnClickAcceptButton);
        
            base.OnViewShow();
        }

        private void OnClickAcceptButton(IButtonView buttonView)
        {
            if (buttonView != null)
            {
                return;
            }
        
            Model.OnClickAccept?.Invoke(buttonView);
        }
    }
}