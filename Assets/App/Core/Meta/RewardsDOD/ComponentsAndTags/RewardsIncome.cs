// Rewards income amount component for sending earned money to specified rewards component.

using Unity.Entities;

namespace App.Core.Meta.RewardsDOD
{
    public struct RewardsIncome : IComponentData
    {
        public readonly RewardsKeys RewardsKey;
        public readonly int IncomeAmount;

        public RewardsIncome(int incomeAmount, RewardsKeys rewardsKey)
        {
            IncomeAmount = incomeAmount;
            RewardsKey = rewardsKey;
        }
    }
}