// Save/Load data for different target/destination types

using System;
using App.Core.Common.Services;
using Cysharp.Threading.Tasks;

namespace App.Core.Storage.StorageService
{
    // Save/Load interface
    public interface IStorageService : IService
    {
        UniTask<bool> LoadAsync(Storages storage, Action<string> onComplete);
        UniTask<bool> KeepAsync(Storages storage, string rawData, Action<bool> onComplete);
        UniTask<bool> SaveAsync(Action<bool> onComplete);
        bool Save(bool forceSave = false);

        public async UniTask<bool> LoadToFixedContainerAsync(Storages storage, Action<FixedStorageContainer> onComplete)
        {
            var result = await LoadAsync(storage, rawDate =>
            {
                if (string.IsNullOrEmpty(rawDate))
                {
                    onComplete?.Invoke(new FixedStorageContainer());
                }
                
                if (FixedStorageContainer.Deserialize(rawDate, out var fixedStorageContainer))
                {
                    onComplete?.Invoke(fixedStorageContainer);
                }
            });

            return result;
        }

        public void LoadToFixedContainer(Storages storage, Action<FixedStorageContainer> onComplete)
        {
            LoadToFixedContainerAsync(storage, onComplete).Forget();
        }

        public void Load(Storages storage, Action<string> onComplete)
        {
            LoadAsync(storage, onComplete).Forget();
        }

        public async UniTask<bool> KeepAsync(Storages storage, FixedStorageContainer fixedStorageContainer,
            Action<bool> onComplete)
        {
            var keepResult = false;
            var rawData = "";

            if (FixedStorageContainer.Serialize(ref fixedStorageContainer, ref rawData))
            {
                keepResult = await KeepAsync(storage, rawData, onComplete);
            }

            fixedStorageContainer.Dispose();

            return keepResult;
        }

        public void Keep(Storages storage, ref FixedStorageContainer fixedStorageContainer, Action<bool> onComplete)
        {
            KeepAsync(storage, fixedStorageContainer, onComplete).Forget();
        }

        public void Keep(Storages storage, ref string rawData, Action<bool> onComplete)
        {
            KeepAsync(storage, rawData, onComplete).Forget();
        }

        protected internal static string GetKeyString(Storages storage)
        {
            return storage.ToString();
        }
    }

    public enum Storages
    {
        Hip,
        StoredValues,
        Profile
    }
}