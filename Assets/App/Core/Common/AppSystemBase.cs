using App.Core.Common.Services;
using Unity.Entities;

namespace App.Core.Common
{
    public abstract partial class AppSystemBase : SystemBase
    {
        private bool initialized;

        protected override void OnUpdate()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;

            if (DIInstaller.GlobalContainer == null) return;

            DIInstaller.GlobalContainer.Inject(this);

            initialized = true;
        }
    }
}