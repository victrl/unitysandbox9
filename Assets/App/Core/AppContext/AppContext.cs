
using System;
using App.AppScenes;
using App.Core.Common.Services;
using App.Core.Meta;
using App.Core.Storage.StorageService;
using App.Core.UI.LocalizationsCustomService;
using App.Core.UI.LocalizationsCustomService.Audio;
using App.Core.UI.PopupService;
using App.Core.UI.SceneTransitionService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ProfileSaveService = App.Core.Storage.ProfileService.ProfileSaveService;

namespace App.Core.AppContext
{
    public class AppContext : InitializationContext
    {
        [Header("Services")] 
        [SerializeField] private SceneTransitionAnimation sceneTransitionAnimation;
        [SerializeField] private AudioStorageService audioStorageService;
        [SerializeField] private UIService uiService;

        public SceneTransitionAnimation SceneTransitionAnimation => sceneTransitionAnimation;
        public AudioStorageService AudioStorageService => audioStorageService;
        private ServicesStorage servicesStorage = new ServicesStorage();

        public static bool IsInitialized { get; private set; }

        protected override void Init()
        {
            sceneTransitionAnimation.ShowLoadingAnimation();

            RegistrationServices();

            base.Init();
        }

        private void RegistrationServices()
        {
            // RegisterService<IStorageService>(new StoragePlayerPrefs());
            RegisterService<IStorageService>(new StorageTxtFile());
            RegisterService<ProfileSaveService>().AddDependency<IStorageService>();
            RegisterService<RewardsService>().AddDependency<ProfileSaveService>();
            RegisterService<SceneTransitionService>();
            RegisterService<LocalizationService>();

            RegisterMonoServices(audioStorageService, true);
            RegisterMonoServices(uiService, true);

            CheckFinishInitialization(() =>
            {
                IsInitialized = true;
                sceneTransitionAnimation.HideLoadingAnimation();
                var sceneTransitionService = servicesStorage.GetService<SceneTransitionService>();
                sceneTransitionService.OpenScene(AppScenes.AppScenes.AppMenu.SceneName());
            }).Forget();
        }

        private T RegisterService<T>() where T : AppService, new()
        {
            var service = ServicesCreator.CreateService<T>(this);
            servicesStorage.RegisterService(service);
            return service;
        }
        
        private T RegisterService<T>(T service) where T : IService
        {
            ServicesCreator.CreateService(this, service);
            servicesStorage.RegisterService(service);
            return service;
        }

        private void RegisterMonoServices<T>(T service, bool asSingle = false) where T : MonoBehaviour, IService
        {
            ServicesCreator.InitMonoServices(service, asSingle);
        }

        public T TryGetService<T>(Type type) where T : class, IService, new()
        {
            return servicesStorage.GetService<T>(type);
        }

        private async UniTaskVoid CheckFinishInitialization(Action onCompleted)
        {
            while (!servicesStorage.ServicesWasInitialized())
            {
                await UniTask.NextFrame();
            }

            onCompleted?.Invoke();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            foreach (var service in servicesStorage)
            {
                (service as AppService)?.OnPause(pauseStatus, DateTime.Now);
            }
        }

        private void OnDestroy()
        {
            foreach (var service in servicesStorage)
            {
                (service as AppService)?.OnCloseApp();
            }
        }
    }
}