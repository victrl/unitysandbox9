
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.AppContext
{
    public abstract class InitializationContext : MonoBehaviour
    {
        private static InitializationContext Instance = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Logger.LogError($"[InitializationContext] => Awake: InitializationContext is more than one");
                return;
            }

            Instance = this;
            Init();
        }
        
        protected virtual void Init()
        {
            Logger.Log($"[InitializationContext] => Init: InitializationContext was initialized");
        }
    }
}
