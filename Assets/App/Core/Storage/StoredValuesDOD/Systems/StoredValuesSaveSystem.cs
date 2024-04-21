// Save stored values from buffer to storage / file

using App.Core.Common;
using App.Core.Meta.RewardsDOD;
using App.Core.Storage.StorageService;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Zenject;
using Logger = App.Common.Tools.Logger;

namespace App.Core.Storage.StoredValuesDOD
{
    [BurstCompile]
    [UpdateAfter(typeof(StoredValuesDOD.StoredValuesBufferSystem))]
    public partial class StoredValuesSaveSystem : AppSystemBase
    {
        [Inject]
        private IStorageService storageService;

        private double nextTimeToSaveStorageToFile;
        // Delay in seconds between saving data to file
        private float deltaTimeToSaveStorageToFile;

        [BurstCompile]
        protected override void OnCreate()
        {
            base.OnCreate();
            
            RequireForUpdate<StoredValuesStorageTag>();
            RequireForUpdate<StoredValuesWasLoadedTag>();
            
            ResetSaveStorageTimer();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            TransferDataFromBufferToStorage();
        }

        protected override void OnStopRunning()
        {
            SaveStorageToFile(true);
            
            base.OnStopRunning();
        }

        [BurstCompile]
        private void ResetSaveStorageTimer(bool stopTimer = false)
        {
            if (deltaTimeToSaveStorageToFile == 0)
            {
                if (SystemAPI.TryGetSingletonEntity<StoredValuesStorageTag>(out var storageEntity))
                {
                    var storageAspect = SystemAPI.GetAspect<StoredValuesDOD.StoredValuesAspect>(storageEntity);
                    deltaTimeToSaveStorageToFile = storageAspect.AutoSaveDelayInSec;
                }
                else
                {
                    deltaTimeToSaveStorageToFile = StoredValuesSettings.StorageSaveDataDelaySecMax;
                }
            }

            if (stopTimer)
            {
                nextTimeToSaveStorageToFile = -1;

                return;
            }

            var delay = deltaTimeToSaveStorageToFile == 0 ? 1.0f : deltaTimeToSaveStorageToFile;
            
            nextTimeToSaveStorageToFile = SystemAPI.Time.ElapsedTime + delay;
        }

        private void SaveStorageToFile(bool forceSave)
        {
            if (!forceSave
                && (nextTimeToSaveStorageToFile < 0
                    || nextTimeToSaveStorageToFile > SystemAPI.Time.ElapsedTime)) return;

            ResetSaveStorageTimer(forceSave);
            
            storageService?.Save(true);
        }
        
        // Transfer all collected values from storage buffer to Storage
        [BurstCompile]
        private void TransferDataFromBufferToStorage()
        {
            if (!SystemAPI.TryGetSingletonEntity<StoredValuesStorageTag>(out var storageEntity)) return;

            var storageAspect = SystemAPI.GetAspect<StoredValuesDOD.StoredValuesAspect>(storageEntity);
            
            if (storageAspect.IsStorageBufferEmpty()) return;

            var storageContainer = new FixedStorageContainer(storageAspect.StorageBuffer.Length, AllocatorManager.Temp);

            foreach (var bufferElement in storageAspect.StorageBuffer)
            {
                storageContainer.Data[new StoredValuesKeyContainer(bufferElement.Key)] = new StoredValuesValueContainer(bufferElement.Value);
            }

            // The buffer is cleared without checking the result of the data transfer,
            // because in any case it will soon be filled with fresh data.
            storageAspect.StorageBuffer.Clear();

            storageService.Keep(Storages.StoredValues,
                ref storageContainer,
                TransferDataFromBufferToStorageCallback);
            
            SaveStorageToFile(false);
        }

        private void TransferDataFromBufferToStorageCallback(bool storageResult)
        {
            if (storageResult) return;

            Logger.LogWarning($"[SaveLoadData.KeepData] => false");
        }
    }
}