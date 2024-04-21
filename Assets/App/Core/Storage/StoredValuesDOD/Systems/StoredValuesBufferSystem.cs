// Collect all the values that need to be stored in a storage component buffer.

using App.Core.Storage.StoredValuesDOD;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(CollectStoredValues<StoredValueInt>))] 

// Example of expanding functionality:
// [assembly: RegisterGenericJobType(typeof(CollectStoredValues<StoredValueFloat>))]
// [assembly: RegisterGenericJobType(typeof(CollectStoredValues<StoredValueVector3>))]

namespace App.Core.Storage.StoredValuesDOD
{
    public partial struct StoredValuesBufferSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StoredValuesStorageTag>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            BufferingStoredValues(ref state);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        // Collect all the values that need to be stored in a storage component buffer.
        [BurstCompile]
        private void BufferingStoredValues(ref SystemState state, bool forceSave = false)
        {
            BufferingStoredValues<StoredValueInt>(ref state);

            // Example of expanding functionality:
            // BufferingStoredValues<StoredValueFloat>(ref state);
            // BufferingStoredValues<StoredValueVector3>(ref state);
        }

        [BurstCompile]
        private void BufferingStoredValues<TStoredComponentType>(ref SystemState state) where TStoredComponentType : unmanaged, IStoredValueComponent, IComponentData
        {
            if (!SystemAPI.TryGetSingletonEntity<StoredValuesStorageTag>(out var storageEntity)) return;

            var storageAspect = SystemAPI.GetAspect<StoredValuesDOD.StoredValuesAspect>(storageEntity);

            if (!storageAspect.IsStorageBufferEmpty()) return;

            if (!SystemAPI.TryGetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>(out var ecbSingleton)) return;

            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var job = new CollectStoredValues<TStoredComponentType>
            {
                //=TODO Warning - Jobs + Generics - waiting for updates from Unity
                StoredComponentType = state.GetComponentTypeHandle<TStoredComponentType>(false),
                StorageEntity = storageEntity,
                Ecb = ecb.AsParallelWriter()
            };
            
            var storedValuesQuery = new EntityQueryBuilder(AllocatorManager.Temp)
                .WithAll<TStoredComponentType>()
                .Build(ref state);

            state.Dependency = job.ScheduleParallel(storedValuesQuery, state.Dependency);
        }
    }

    // Collect all the values that need to be stored in a storage component buffer.
    [BurstCompile]
    public struct CollectStoredValues<TStoredComponentType> : IJobChunk where TStoredComponentType : unmanaged, IStoredValueComponent
    {
        public Entity StorageEntity;
        public ComponentTypeHandle<TStoredComponentType> StoredComponentType;
        public EntityCommandBuffer.ParallelWriter Ecb;

        [BurstCompile]
        public void Execute(in ArchetypeChunk chunk,
            int unfilteredChunkIndex,
            bool useEnableMask,
            in v128 chunkEnabledMask)
        {
            var storedValueComponents = chunk.GetNativeArray(ref StoredComponentType);

            var enumerator = new ChunkEntityEnumerator(useEnableMask, chunkEnabledMask, chunk.Count);

            while (enumerator.NextEntityIndex(out var i))
            {
                if (i >= storedValueComponents.Length) continue;
                
                var storedValueComponent = storedValueComponents[i];
                var incomeElement = new StoredValuesBufferElement
                    { Key = storedValueComponent.KeyContainer.Key, Value = storedValueComponent.StoredValue };
                Ecb.AppendToBuffer(unfilteredChunkIndex, StorageEntity, incomeElement);
            }
        }
    }
}
