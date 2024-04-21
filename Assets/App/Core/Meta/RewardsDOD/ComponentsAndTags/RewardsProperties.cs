using Unity.Entities;

namespace App.Core.Meta.RewardsDOD
{
    public struct RewardsProperties : IComponentData
    {
        public RewardsKeys RewardKey;
    }

    public enum RewardsKeys
    {
        Coins,
        Crystals,
        RewardsKeysCount
    }
}