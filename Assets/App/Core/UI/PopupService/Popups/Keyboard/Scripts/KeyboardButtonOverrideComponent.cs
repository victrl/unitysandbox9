using App.Common.UI.Elements.Buttons;
using UnityEngine;

namespace App.Core.UI.PopupService.Popups
{
    public class KeyboardButtonOverrideComponent : MonoBehaviour, IButtonOverrideComponent
    {
        [SerializeField] private int numberOfKey;
        [SerializeField] private KeyType keyType;

        public string GetDefaultText()
        {
            return numberOfKey.ToString();
        }

        public T GetAdditionalData<T>() where T : class
        {
            KeyData result = new KeyData()
            {
                KeyType = keyType,
                KeyValue = numberOfKey,
            };
        
            if(result is T keyDif)
            {
                return keyDif;
            }

            return null;
        }
    }
}