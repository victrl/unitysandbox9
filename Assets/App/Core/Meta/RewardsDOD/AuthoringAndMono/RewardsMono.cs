// Rewards components baker.

using App.Core.Storage.StoredValuesDOD;
using Unity.Entities;
using UnityEngine;

namespace App.Core.Meta.RewardsDOD
{
    public class RewardsMono : MonoBehaviour
    {
        public string keyPrefix = "Rewards"; 
        public RewardsKeys rewardKey;
    }

    public class RewardsBaker : Baker<RewardsMono>
    {
        public override void Bake(RewardsMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<StoredValuesWaitingForLoadTag>(entity);
            AddBuffer<RewardsIncomeBufferElement>(entity);
            AddComponent(entity, new RewardsProperties
            {
                RewardKey = authoring.rewardKey
            });
            AddComponent(entity,
                new StoredValueInt(new StoredValuesKeyContainer(authoring.keyPrefix + authoring.rewardKey)));
        }
    }
}