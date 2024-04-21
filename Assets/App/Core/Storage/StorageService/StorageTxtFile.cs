using System;
using System.Collections.Generic;
using System.IO;
using App.Core.Common.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Storage.StorageService
{
    public class StorageTxtFile : AppService, IStorageService
    {
        private static class SerializedDataStorage
        {
            private static Dictionary<string, string> serializedDataStorage;

            private static bool isLocked;

            public static async UniTask<bool> KeepAsync(string key, string value)
            {
                var tryCount = 8;
                while (isLocked && tryCount > 0)
                {
                    await UniTask.NextFrame();
                    tryCount--;
                }
                
                if (isLocked) return false;

                isLocked = true;
                
                serializedDataStorage ??= new Dictionary<string, string>();

                serializedDataStorage[key] = value;

                isLocked = false;

                return true;
            }
            
            public static async UniTask<Dictionary<string, string>> CopyAsync()
            {
                var tryCount = 8;
                while ((isLocked || serializedDataStorage == null) && tryCount > 0)
                {
                    await UniTask.NextFrame();
                    tryCount--;
                }

                if (isLocked || serializedDataStorage == null) return null;

                isLocked = true;

                var tempStorage = new Dictionary<string, string>(serializedDataStorage); 

                isLocked = false;

                return tempStorage;
            }
            
            public static Dictionary<string, string> Copy(bool forceCopy = false)
            {
                if ((isLocked && !forceCopy) || serializedDataStorage == null) return null;

                isLocked = true;

                var tempStorage = new Dictionary<string, string>(serializedDataStorage); 

                isLocked = false;

                return tempStorage;
            }
        }

        private const string CommonPath = "App/Editor/Saves";
        private const string FileExt = ".txt";

        private bool initialized;
        private bool inProcess;
        
        public async UniTask<bool> LoadAsync(Storages storage, Action<string> onComplete)
        {
            var path = ComposePath(IStorageService.GetKeyString(storage));

            try
            {
                if (!File.Exists(path))
                {
                    Logger.LogWarning($"[StorageTxtFile] => Load file not found: {path}");

                    onComplete?.Invoke(default);

                    return false;
                }

                await File.ReadAllTextAsync(path).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        onComplete?.Invoke(t.Result);
                    }
                    else if (t.IsFaulted)
                    {
                        Logger.LogWarning($"[StorageTxtFile] => Load IsFaulted {t}");
                    
                        onComplete?.Invoke(default);
                    }
                
                    return t.IsCompleted;
                });
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[StorageTxtFile] => {e.Message}");

                return false;
            }
            
            return false;
        }

        public async UniTask<bool> KeepAsync(Storages storage, string rawData, Action<bool> onComplete)
        {
            var keepResult = await SerializedDataStorage.KeepAsync(IStorageService.GetKeyString(storage), rawData);
    
            onComplete?.Invoke(keepResult);

            return keepResult;
        }

        public async UniTask<bool> SaveAsync(Action<bool> onComplete)
        {
            if (inProcess) return false;
            inProcess = true;
            
            var tempStorage = await SerializedDataStorage.CopyAsync();

            if (tempStorage == null)
            {
                onComplete?.Invoke(false);
                
                inProcess = false;

                return false;
            }
            
            var saveResult = true;

            foreach (var storedItem in tempStorage)
            {
                var path = storedItem.Key;
                var rawData = storedItem.Value;

                try
                {
                    if (!File.Exists(path))
                    {
                        await File.CreateText(path).DisposeAsync();
                    }

                    await File.WriteAllTextAsync(path, rawData).ContinueWith((t) =>
                    {
                        if (t.IsFaulted)
                        {
                            Logger.LogWarning($"[StorageTxtFile] => Save IsFaulted {t}");
                        }

                        saveResult = saveResult && t.IsCompleted;
                    });
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"[StorageTxtFile] => Save error {e.Message}");
                    
                    saveResult = false;
                }
            }
            
            onComplete?.Invoke(saveResult);

            inProcess = false;

            return saveResult;
        }

        public bool Save(bool forceSave = false)
        {
            var tempStorage = SerializedDataStorage.Copy(forceSave);

            if (tempStorage == null)
            {
                return false;
            }
            
            var saveResult = true;
            
            foreach (var storedItem in tempStorage)
            {
                var path = ComposePath(storedItem.Key);
                var rawData = storedItem.Value;

                try
                {
                    if (!File.Exists(path))
                    {
                        File.CreateText(path);
                    }

                    File.WriteAllText(path, rawData);
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"[StorageTxtFile] => Save error {e.Message}");
                    
                    saveResult = false;
                }
            }

            return saveResult;
        }

        private static string ComposePath(string storageKey)
        {
            return Path.Combine(Application.dataPath, CommonPath, storageKey + FileExt);
        }
    }
}