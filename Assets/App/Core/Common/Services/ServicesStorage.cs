
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Common.Services
{
    public class ServicesStorage : IEnumerable<IService>, IEnumerator<IService>
    {
        public IService Current => currentService;
        object IEnumerator.Current => Current;
        
        private readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();
        
        public T RegisterService<T>(T service) where T : IService
        {
            service.OnRegister();
            service.PrepareService();
            services.Add(typeof(T), service);
            return service;
        }

        public void UnregisterService<T>() where T : class, IService, new()
        {
            var service = GetService<T>();
            service.OnUnregister();
            services.Remove(typeof(T));
        }

        public T GetService<T>() where T : class, IService, new()
        {
            return GetService<T>(typeof(T));
        }

        public T GetService<T>(Type type) where T : class, IService, new() 
        {
            if (services.TryGetValue(type, out var neededService) && neededService != null)
            {
                return neededService as T;
            }

            Logger.LogWarning($"[AppInitializationContext] => GetService: neededService by {typeof(T)} isn't registered");
            return null;
        }

        public bool ServicesWasInitialized()
        {
            return services.All(service => service.Value.IsInitialized);
        }

        private int currentIndex = -1;
        private IService currentService;
        
        public bool MoveNext()
        {
            if (services == null)
            {
                return false;
            }
            
            if (++currentIndex >= services.Count)
            {
                return false;
            }

            currentService = services.ElementAtOrDefault(currentIndex).Value;
            return true;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public void Dispose()
        {
            Reset();
        }

        public IEnumerator<IService> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}