// Loading stored values from file and place it to components

using App.Core.Common;
using App.Core.Meta.RewardsDOD;
using App.Core.Storage.StorageService;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Zenject;

namespace App.Core.Storage.StoredValuesDOD
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class StoredValuesLoadSystem : AppSystemBase
    {
        [Inject]
        private IStorageService storageService;

        private FixedStorageContainer loadedFixedStorageContainer;

        [BurstCompile]
        protected override void OnCreate()
        {
            base.OnCreate();
            
            RequireForUpdate<StoredValuesStorageTag>();
            RequireForUpdate<StoredValuesWaitingForLoadTag>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!loadedFixedStorageContainer.IsReady)
            {
                LoadStorageFromFile();
            }
            else
            {
                PublishValuesToEntities(ref loadedFixedStorageContainer);
            }
        }

        [BurstCompile]
        private void LoadStorageFromFile()
        {
            storageService?.LoadToFixedContainer(Storages.StoredValues, storageContainer =>
            {
                if (storageContainer.IsEmpty)
                {
                    storageContainer.Dispose();
                    
                    return;
                }
                
                loadedFixedStorageContainer = storageContainer;
            });
        }

        // Place all restored values to components
        [BurstCompile]
        private void PublishValuesToEntities(ref FixedStorageContainer fixedStorageContainer)
        {
            if (!fixedStorageContainer.IsReady) return;

            if (!PublishValuesToEntities<StoredValueInt>(ref fixedStorageContainer)) return;
            
            // Example of expanding functionality:
            // PublishValuesToEntities<StoredValueFloat>();
            // PublishValuesToEntities<StoredValueVector3>();
            
            fixedStorageContainer.Dispose(Dependency);
        }

        //=TODO Warning: Compilation was requested for method ... but it is not a known Burst entry point.
        // This may be because the [BurstCompile] method is defined in a generic class,
        // and the generic class is not instantiated with concrete types anywhere in your code.
        // 
        // Waiting for better generics support
        [BurstCompile]
        private bool PublishValuesToEntities<TStoredComponentType>(ref FixedStorageContainer fixedStorageContainer) where TStoredComponentType : unmanaged, IStoredValueComponent, IComponentData
        {
            var storedValuesQuery = new EntityQueryBuilder(AllocatorManager.TempJob)
                .WithAll<TStoredComponentType>()
                .WithAll<StoredValuesWaitingForLoadTag>()
                .Build(this);

            if (storedValuesQuery.CalculateEntityCount() < 1) return false;

            if (!SystemAPI.TryGetSingletonEntity<StoredValuesStorageTag>(out var storageEntity)) return false;

            if (!SystemAPI.TryGetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>(out var ecbSingleton)) return false;

            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);

            var job = new PublishValuesToEntities<TStoredComponentType>
            {
                StoredComponentType = GetComponentTypeHandle<TStoredComponentType>(false),
                StoredComponentEntityHandle = GetEntityTypeHandle(), 
                Storage = fixedStorageContainer,
                Ecb = ecb.AsParallelWriter(),
                StorageEntity = storageEntity
            };
            
            Dependency = job.ScheduleParallel(storedValuesQuery, Dependency);
            
            return true;
        }
    }
    
    // Place all restored values to components
    [BurstCompile]
    public struct PublishValuesToEntities<TStoredComponentType> : IJobChunk where TStoredComponentType : unmanaged, IStoredValueComponent
    {
        [ReadOnly] public FixedStorageContainer Storage;
        public EntityTypeHandle StoredComponentEntityHandle;
        public ComponentTypeHandle<TStoredComponentType> StoredComponentType;
        public EntityCommandBuffer.ParallelWriter Ecb;
        public Entity StorageEntity;

        [BurstCompile]
        public void Execute(in ArchetypeChunk chunk,
            int unfilteredChunkIndex,
            bool useEnableMask,
            in v128 chunkEnabledMask)
        {
            var storedValueComponents = chunk.GetNativeArray(ref StoredComponentType);
            var storedValueEntities = chunk.GetNativeArray(StoredComponentEntityHandle);

            var enumerator = new ChunkEntityEnumerator(useEnableMask, chunkEnabledMask, chunk.Count);

            while (enumerator.NextEntityIndex(out var i))
            {
                if (i >= storedValueComponents.Length) continue;
                
                var storedValueComponent = storedValueComponents[i];
                var storedValueEntity = storedValueEntities[i];

                if (Storage.Data.TryGetValue(storedValueComponent.KeyContainer, out var restoredValue))
                {
                    storedValueComponent.StoredValue = restoredValue.Value;
                    Ecb.SetComponent(unfilteredChunkIndex, storedValueEntity, storedValueComponent);
                }
                Ecb.RemoveComponent<StoredValuesWaitingForLoadTag>(unfilteredChunkIndex, storedValueEntity);
            }
            Ecb.AddComponent<StoredValuesWasLoadedTag>(unfilteredChunkIndex, StorageEntity);
        }
    }
}