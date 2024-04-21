
using System;
using System.Collections.Generic;
using App.Core.Storage.StoredValuesDOD;
using Unity.Collections;
using Unity.Jobs;
using Unity.Serialization.Json;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Storage.StorageService
{
    // Container for transfer loaded from file data to destination components for values restoration
    public struct FixedStorageContainer
    {
        public NativeHashMap<StoredValuesKeyContainer, StoredValuesValueContainer> Data;

        public bool IsEmpty => (!Data.IsCreated || Data.IsEmpty);

        // Data is ready to process
        public bool IsReady { get; set; }

        public FixedStorageContainer(int capacity, AllocatorManager.AllocatorHandle allocatorManager)
        {
            IsReady = false;
            
            Data = new NativeHashMap<StoredValuesKeyContainer, StoredValuesValueContainer>(capacity, allocatorManager);
        }

        public void Dispose(JobHandle jobHandle)
        {
            IsReady = false;

            if (Data.IsCreated)
            {
                Data.Dispose(jobHandle);
            }
        }

        public void Dispose()
        {
            IsReady = false;

            if (Data.IsCreated)
            {
                Data.Dispose();
            }
        }

        // StorageContainer HashMap to string
        public static bool Serialize(ref FixedStorageContainer fixedStorageContainer, ref string rawData)
        {
            try
            {
                var storageForSerialization = new Dictionary<string, string>();

                foreach (var item in fixedStorageContainer.Data)
                {
                    storageForSerialization[item.Key.Key.ToString()] = item.Value.Value.ToString();
                }
                
                rawData = JsonSerialization.ToJson(storageForSerialization);

                return !string.IsNullOrEmpty(rawData);
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[StorageContainer.Serialize] => {e.Message}");

                return false;
            }
        }
        
        // string to StorageContainer HashMap
        public static bool Deserialize(string rawData, out FixedStorageContainer fixedStorageContainer)
        {
            try
            {
                var storageManaged = JsonSerialization.FromJson<Dictionary<string, string>>(rawData);

                if (storageManaged == null || storageManaged.Count == 0)
                {
                    fixedStorageContainer = default;
                    
                    return false;
                }

                fixedStorageContainer = new FixedStorageContainer(storageManaged.Count, AllocatorManager.TempJob);
                    
                foreach (var item in storageManaged)
                {
                    fixedStorageContainer.Data[new StoredValuesKeyContainer(item.Key)] = new StoredValuesValueContainer(item.Value);
                }
                    
                storageManaged.Clear();

                fixedStorageContainer.IsReady = true;
                
                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning($"[StorageContainer.Deserialize] => {e.Message}");

                fixedStorageContainer = default;

                return false;
            }
        }
    }
}