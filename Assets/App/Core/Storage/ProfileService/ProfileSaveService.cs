using System;
using System.Collections.Generic;
using App.Common.Tools;
using App.Core.Common.Services;
using App.Core.Storage.StorageService;
using Newtonsoft.Json;
using Zenject;

namespace App.Core.Storage.ProfileService
{
    public partial class ProfileSaveService : AppService
    {
        [Inject] 
        private IStorageService storageService;

        private Dictionary<Type, SaveComponent> components = new ();
        
        protected override void FinishInitialize()
        {
            LoadProfileData((success) =>
            {
                Logger.Log($"[ProfileSaveService] => FinishInitialize: LoadProfileData success - {success}");

                RegistrationSaveComponents();

                base.FinishInitialize();
            });
        }

        public T GetSaveComponent<T>() where T : SaveComponent
        {
            if (components.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }

            Logger.LogError($"[ProfileSaveService] => GetSaveComponent: Type {typeof(T)} isn't contains");
            return null;
        }

        public override void OnPause(bool pauseStatus, DateTime pauseStartedTime)
        {
            base.OnPause(pauseStatus, pauseStartedTime);

            if (pauseStatus)
            {
                SaveProfileData();
            }
        }

        public override void OnCloseApp()
        {
            SaveProfileData();
            base.OnCloseApp();
        }

        private void SaveProfileData()
        {
            var containers = new List<ComponentContainer>();

            foreach (var component in components)
            {
                var componentType = component.Value.GetType();
                string typeAsString = componentType.ToString();
                
                containers.Add(new ComponentContainer()
                {
                    Type = typeAsString,
                    Data = JsonConvert.SerializeObject(component.Value),
                });
            }

            var rawData = JsonConvert.SerializeObject(containers, Formatting.Indented);

            storageService?.Keep(Storages.Profile, ref rawData, success =>
            {
                if (success)
                {
                    Logger.Log($"[ProfileSaveService] => SaveProfileData: success \n {rawData}");
                }
                else
                {
                    Logger.LogWarning($"[ProfileSaveService] => SaveProfileData: failed \n {rawData}");
                }
            });
        }

        private void LoadProfileData(Action<bool> onLoaded)
        {
            storageService.Load(Storages.Profile, rawData =>
            {
                try
                {
                    if (string.IsNullOrEmpty(rawData))
                    {
                        onLoaded.Invoke(false);

                        return;
                    }
                    
                    components = new Dictionary<Type, SaveComponent>();
                    var containers = JsonConvert.DeserializeObject<List<ComponentContainer>>(rawData);

                    foreach (var container in containers)
                    {
                        var componentType = Type.GetType(container.Type);
                        var component = JsonConvert.DeserializeObject(container.Data, componentType);
                        
                        if (componentType != null && component != null)
                        {
                            Logger.Log($"[ProfileSaveService] => LoadProfileData: loaded {componentType}");
                            components.Add(componentType, component as SaveComponent);
                        }
                    }
                    
                    Logger.Log($"[ProfileSaveService] => LoadProfileData: \n {rawData}");
                    onLoaded.Invoke(true);
                }
                catch (Exception e)
                {
                    Logger.LogError($"[ProfileSaveService] => LoadProfileData: {e.GetInfo()}");
                    onLoaded.Invoke(false);
                }
            });
        }

        private void OnProfileDataLoaded(string rawData)
        {
            
        }
    }
}