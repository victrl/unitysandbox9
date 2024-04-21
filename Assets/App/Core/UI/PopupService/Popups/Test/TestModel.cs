using System;
using PopupSystem;

namespace App.Core.UI.PopupService.Popups
{
    public class TestModel : PopupModel
    {
        public Action<IButtonView> OnClickOk;
    }
}