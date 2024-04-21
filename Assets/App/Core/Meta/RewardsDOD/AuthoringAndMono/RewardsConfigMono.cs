using Unity.Entities;
using UnityEngine;

namespace App.Core.Meta.RewardsDOD
{
    public class RewardsConfigMono : MonoBehaviour
    {
        class Baker : Baker<RewardsConfigMono>
        {
            public override void Bake(RewardsConfigMono mono)
            {
                var entity = GetEntity(mono, TransformUsageFlags.None);

                AddComponent(entity, new RewardsConfigData());
                
                AddComponentObject(entity, new RewardsConfigManaged());
            }
        }
    }
    
    public struct RewardsConfigData : IComponentData {}

    public class RewardsConfigManaged : IComponentData
    {
        public RewardsUIMono RewardsUIMono;
    }
}
