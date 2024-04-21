using App.Common.Tools;
using App.Core.Common.Services;
using Cysharp.Threading.Tasks;

namespace App.Core.UI.PopupService
{
    public sealed class UIService : PopupSystem.PopupService, IService
    {
        private AppContext.AppContext context;
        
        public bool IsInitialized { get; private set; }

        public void Inject()
        {
            DIInstaller.GlobalContainer.Inject(this);
            FinishInitialize();
        }

        private void FinishInitialize()
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
    }
}