using App.Core.Storage;
using App.Core.Storage.StoredValuesDOD;
using Unity.Entities;
using Unity.Transforms;

namespace App.Core.Meta.RewardsDOD
{
    public readonly partial struct RewardsAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> transform;
        private readonly RefRW<RewardsProperties> rewardsProperties;
        private readonly RefRW<StoredValueInt> storedValue;
        private readonly DynamicBuffer<RewardsIncomeBufferElement> rewardsIncomeBuffer;

        public int Amount => storedValue.ValueRO.Value;
        public RewardsKeys RewardsKey => rewardsProperties.ValueRO.RewardKey;

        public RefRW<RewardsProperties> RewardsProperties => rewardsProperties;

        // Add income amount from IncomeBuffer to rewards stored values.
        public void ProcessIncomeBuffer()
        {
            foreach (var rewardsIncome in rewardsIncomeBuffer)
            {
                if (rewardsIncome.Value != 0)
                {
                    storedValue.ValueRW.Value += rewardsIncome.Value;
                }
            }

            rewardsIncomeBuffer.Clear();
        }
    }
}