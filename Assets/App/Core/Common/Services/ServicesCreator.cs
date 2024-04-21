
using UnityEngine;

namespace App.Core.Common.Services
{
    public class ServicesCreator
    {
        public static T CreateService<T>(AppContext.AppContext context, bool asSingle = false) where T : AppService, new()
        {
            var service = new T();
            service.SetContext(context);
            var binder = DIInstaller.GlobalContainer.Bind<T>().FromInstance(service);

            if (asSingle)
            {
                binder.AsSingle();
            }

            return service;
        }
        
        public static T CreateService<T>(AppContext.AppContext context, T service, bool asSingle = false) where T : IService
        {
            service.SetContext(context);
            var binder = DIInstaller.GlobalContainer.Bind<T>().FromInstance(service);

            if (asSingle)
            {
                binder.AsSingle();
            }

            return service;
        }

        public static T InitMonoServices<T>(T service, bool asSingle = false) where T : MonoBehaviour, IService
        {
            var binder = DIInstaller.GlobalContainer.Bind<T>().FromInstance(service);

            if (asSingle)
            {
                binder.AsSingle();
            }

            return service;
        }
    }
}