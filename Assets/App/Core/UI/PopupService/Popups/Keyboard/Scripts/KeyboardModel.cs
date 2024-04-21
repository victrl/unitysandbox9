using System;
using PopupSystem;

namespace App.Core.UI.PopupService.Popups
{
    public class KeyboardModel : PopupModel
    {
        public Action<int> OnClickAccept;
        public int MaxCharInScreen;
    }
}