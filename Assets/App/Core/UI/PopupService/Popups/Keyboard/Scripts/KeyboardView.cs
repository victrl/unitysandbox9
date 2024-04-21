using System.Collections.Generic;
using App.Common.UI.Elements.Buttons;
using PopupSystem;
using TMPro;
using UnityEngine;

namespace App.Core.UI.PopupService.Popups
{
    public class KeyboardView : PopupView<KeyboardModel>
    {
        [SerializeField] private TMP_Text screenView;
        [SerializeField] private List<ButtonView> buttons;

        protected override void OnViewShow()
        {
            screenView.text = string.Empty;
        
            foreach (var button in buttons)
            {
                InitButton(button, OnClickButton);
            }
        
            base.OnViewShow();
        }

        private void OnClickButton(IButtonView buttonView)
        {
            var data = buttonView.GetAdditionalData<KeyData>();

            if (data == null)
            {
                return;
            }

            if (data.KeyType == KeyType.Number)
            {
                InputText(data.KeyValue.ToString());
                return;
            }

            if (data.KeyType == KeyType.Action)
            {
                if (data.KeyValue == -1)
                {
                    CleanLastSymbol();
                    return;
                }

                if (string.IsNullOrEmpty(screenView.text))
                {
                    Model.OnClickAccept?.Invoke(0);
                    return;
                }
            
                int keyboardValue = int.Parse(screenView.text);
                Model.OnClickAccept?.Invoke(keyboardValue);
            }
        }

        private void InputText(string number)
        {
            if (screenView.text.Length >= Model.MaxCharInScreen)
            {
                return;
            }

            screenView.text += number;
        }
    
        private void CleanLastSymbol()
        {
            if (screenView.text.Length == 0)
            {
                Model.OnClickAccept?.Invoke(0);
                return;
            }

            screenView.text = screenView.text.Remove(screenView.text.Length - 1, 1);
        }
    }
}