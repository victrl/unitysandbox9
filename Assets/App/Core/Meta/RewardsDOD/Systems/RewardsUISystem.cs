using App.Core.Storage;
using App.Core.Storage.StoredValuesDOD;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace App.Core.Meta.RewardsDOD
{
    [BurstCompile]
    public partial class RewardsUISystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<StoredValuesStorageTag>();
            RequireForUpdate<RewardsConfigData>();
            RequireForUpdate<RewardsProperties>();
        }

        [BurstDiscard]
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<RewardsConfigData>(out var configEntity)) return;

            var configManaged = EntityManager.GetComponentObject<RewardsConfigManaged>(configEntity);
            
            if (configManaged.RewardsUIMono == null)
            {
                configManaged.RewardsUIMono = Object.FindObjectOfType<RewardsUIMono>();
            }
            
            if (configManaged.RewardsUIMono == null) return;
            
            Entities.WithChangeFilter<RewardsProperties>().ForEach((RewardsAspect rewardsAspect) =>
            {
                //=TODO need to fix Burst warning (f.e. send all values to mono without a loop)
                configManaged.RewardsUIMono.UpdateRewardsValue(rewardsAspect.Amount, rewardsAspect.RewardsKey);

            }).WithoutBurst().Run();
        }
    }
}