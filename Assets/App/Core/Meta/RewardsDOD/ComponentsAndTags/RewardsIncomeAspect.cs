using Unity.Entities;
using Unity.Transforms;
using IAspect = Unity.Entities.IAspect;

namespace App.Core.Meta.RewardsDOD
{
    public readonly partial struct RewardsIncomeAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> transform;
        private readonly RefRW<RewardsIncome> rewardsIncome;
        
        public int IncomeAmount => rewardsIncome.ValueRO.IncomeAmount;
        public RewardsKeys RewardsKey => rewardsIncome.ValueRO.RewardsKey;
    }
}
