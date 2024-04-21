
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Common.Services
{
    public class AppService : IService
    {
        public bool IsInitialized { get; private set; }
        protected AppContext.AppContext Context { get; private set; }
        private readonly List<Type> dependencyTypes = new List<Type>();
        private readonly List<AppService> dependencies = new List<AppService>();

        public void SetContext(AppContext.AppContext context)
        {
            Context = context;
        }

        public AppService AddDependency<T>()
        {
            dependencyTypes.Add(typeof(T));
            return this;
        }

        public async UniTaskVoid PrepareService()
        {
            dependencies.Clear();

            do
            {
                await UniTask.NextFrame();

                if (dependencyTypes == null || dependencyTypes.Count == 0) break;

                for (var i = dependencyTypes.Count - 1; i >= 0; i--)
                {
                    var dependencyType = dependencyTypes[i];
                    
                    var service = Context.TryGetService<AppService>(dependencyType);

                    if (service is { IsInitialized: true })
                    {
                        dependencies.Add(service);

                        dependencyTypes.RemoveAt(i);
                    }
                }

            } while (dependencyTypes.Count > 0);

            Initialization();
        }

        protected virtual void Initialization()
        {
            Inject();
            FinishInitialize();
        }

        protected virtual void FinishInitialize()
        {
            IsInitialized = true;
            Logger.Log($"[{GetType()}] => Initialization: Finished");
        }

        public virtual void OnRegister()
        {
            Logger.Log($"[{GetType()}] => OnRegister");
        }

        public virtual void OnUnregister()
        {
            Logger.LogError($"[{GetType()}] => OnUnregister");
        }

        public virtual void OnPause(bool pauseStatus, DateTime pauseStartedTime)
        {
        }

        public virtual void OnCloseApp()
        {
        }

        public virtual void Inject()
        {
            DIInstaller.GlobalContainer.Inject(this);
        }
    }

    public class EmptyService : AppService
    {
    }
}