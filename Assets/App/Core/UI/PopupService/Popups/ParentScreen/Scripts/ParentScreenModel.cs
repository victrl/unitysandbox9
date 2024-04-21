using System;
using PopupSystem;

namespace App.Core.UI.PopupService.Popups
{
    public class ParentScreenModel : PopupModel
    {
        public Action<IButtonView> OnClickAccept;
    }
}