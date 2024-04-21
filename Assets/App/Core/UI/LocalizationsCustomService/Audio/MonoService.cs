using App.Core.Common.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.UI.LocalizationsCustomService.Audio
{
    public class MonoService : MonoBehaviour, IService
    {
        private AppContext.AppContext context;
    
        public virtual void Inject()
        {
            DIInstaller.GlobalContainer.Inject(this);
            FinishInitialize();
        }

        protected virtual void FinishInitialize()
        {
            IsInitialized = true;
        }

        public void OnRegister()
        {
            Logger.Log($"[{GetType()}] => OnRegister");
        }

        public void SetContext(AppContext.AppContext context)
        {
            this.context = context;
        }

        public UniTaskVoid PrepareService()
        {
            Logger.Log($"[{GetType()}] => PrepareService");

            return default;
        }

        public void OnUnregister()
        {
            Logger.Log($"[{GetType()}] => OnUnregister");
        }

        public bool IsInitialized { get; private set; }
    }
}