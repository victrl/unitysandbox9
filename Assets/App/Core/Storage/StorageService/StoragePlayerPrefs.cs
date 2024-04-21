using System;
using App.Core.Common.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Storage.StorageService
{
    public class StoragePlayerPrefs : AppService, IStorageService
    {
        public async UniTask<bool> LoadAsync(Storages storage, Action<string> onComplete)
        {
            await UniTask.SwitchToMainThread();

            if (!PlayerPrefs.HasKey(IStorageService.GetKeyString(storage)))
            {
                onComplete?.Invoke(default);
                
                return false;
            }

            try
            {
                var rawData = PlayerPrefs.GetString(IStorageService.GetKeyString(storage), string.Empty);

                onComplete?.Invoke(rawData);

                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[StoragePlayerPrefs] => Failed to load {e}");

                onComplete?.Invoke(default);

                return false;
            }
        }

        public async UniTask<bool> KeepAsync(Storages storage, string rawData, Action<bool> onComplete)
        {
            if (string.IsNullOrEmpty(rawData))
            {
                onComplete?.Invoke(false);

                return false;
            }

            await UniTask.SwitchToMainThread();

            try
            {
                PlayerPrefs.SetString(IStorageService.GetKeyString(storage), rawData);
            
                onComplete?.Invoke(true);
                
                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[StoragePlayerPrefs] => Failed to store {e}");

                onComplete?.Invoke(false);
                
                return false;
            }
        }

        public async UniTask<bool> SaveAsync(Action<bool> onComplete)
        {
            await UniTask.SwitchToMainThread();

            var saveResult = Save();
            
            onComplete?.Invoke(saveResult);

            return saveResult;
        }

        public bool Save(bool forceSave = false)
        {
            try
            {
                PlayerPrefs.Save();

                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[StoragePlayerPrefs] => Failed to save {e}");

                return false;
            }
        }
    }
}