using App.Common.UI.Elements.Buttons;
using PopupSystem;
using UnityEngine;

namespace App.Core.UI.PopupService.Popups
{
    public class TestView : PopupView<TestModel>
    {
        [SerializeField] private ButtonView goButton;

        protected override void OnViewShow()
        {
            base.OnViewShow();
            InitButton(goButton, Model.OnClickOk);
        }
    }
}