// A buffer element to transfer earned rewards from the source event to the specified target rewards components.

using Unity.Entities;

namespace App.Core.Meta.RewardsDOD
{
    [InternalBufferCapacity(8)]
    public struct RewardsIncomeBufferElement : IBufferElementData
    {
        public int Value;
    }
}
