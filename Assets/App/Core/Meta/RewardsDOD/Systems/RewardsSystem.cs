// Collect all rewards income from source events to rewards income buffer.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace App.Core.Meta.RewardsDOD
{
    [BurstCompile]
    public partial struct RewardsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RewardsConfigData>();
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            CollectRewardsIncomeToBuffer(ref state);
        }

        [BurstCompile]
        private void CollectRewardsIncomeToBuffer(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            var rewardsEntities = SystemAPI.QueryBuilder().WithAll<RewardsProperties>().Build()
                .ToEntityArray(AllocatorManager.TempJob);

            
            if (!rewardsEntities.IsCreated || rewardsEntities.Length == 0) return;

            // Collect income for each rewards type.
            var collectRewardsJob = new CollectRewardsIncomeJob
            {
                RewardsEntities = rewardsEntities,
                RewardsPropertiesLookup = SystemAPI.GetComponentLookup<RewardsProperties>(),
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            };

            var setRewardsJob = new SetRewardsIncomeJob();
            var collectRewardsJobHandle = collectRewardsJob.Schedule(state.Dependency);
            var setRewardsJobHandle = setRewardsJob.Schedule(collectRewardsJobHandle);
            setRewardsJobHandle.Complete();
        }

        // Collect income from all source earn events to Income Buffer on the target rewards entity.
        [BurstCompile]
        public partial struct CollectRewardsIncomeJob : IJobEntity
        {
            [DeallocateOnJobCompletion]
            public NativeArray<Entity> RewardsEntities;
            
            public ComponentLookup<RewardsProperties> RewardsPropertiesLookup;
            
            public EntityCommandBuffer.ParallelWriter ECB;

            [BurstCompile]
            private void Execute(RewardsDOD.RewardsIncomeAspect income, [EntityIndexInChunk]int sortKey)
            {
                var incomeAmount = income.IncomeAmount;

                if (incomeAmount <= 0) return;
                
                foreach (var entity in RewardsEntities)
                {
                    var rewardsProperties = RewardsPropertiesLookup[entity];

                    if (income.RewardsKey != rewardsProperties.RewardKey) continue;
                    
                    var incomeElement = new RewardsIncomeBufferElement { Value = incomeAmount };
                    ECB.AppendToBuffer(sortKey, entity, incomeElement);
                
                    // remove income components after processing
                    ECB.RemoveComponent<RewardsIncome>(sortKey, income.Entity);
                }
            }
        }
        
        [BurstCompile]
        public partial struct SetRewardsIncomeJob : IJobEntity
        {
            [BurstCompile]
            private void Execute(RewardsDOD.RewardsAspect rewardsAspect, [EntityIndexInChunk]int sortKey)
            {
                rewardsAspect.ProcessIncomeBuffer();
            }
        }
    }
}