using UnityEngine;

namespace App.Common.UI.Elements.Buttons
{
    public class DefaultButtonOverrideComponent : MonoBehaviour, IButtonOverrideComponent
    {
        [SerializeField] private string customData;
        
        public string GetDefaultText() => string.Empty;

        public T GetAdditionalData<T>() where T : class
        {
            if(customData is T data)
            {
                return data;
            }

            return null;
        }
    }
}