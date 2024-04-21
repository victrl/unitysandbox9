using System;
using System.Collections.Generic;
using App.Common.Tools;
using App.Core.Meta;

namespace App.Core.Storage.ProfileService
{
    public partial class ProfileSaveService
    {
        private void RegistrationSaveComponents()
        {
            Register<RewardSaveComponent>();
        }

        private void Register<T>() where T : SaveComponent, new()
        {
            if (components == null)
            {
                components = new Dictionary<Type, SaveComponent>();
            }
            
            if (components.TryGetValue(typeof(T), out var component))
            {
                Logger.Log($"[ProfileSaveService] => Register: {typeof(T)} was registered: \n {component}");
                return;
            }

            components.Add(typeof(T), new T());
        }
    }
}