using App.Core.Meta;
using App.Core.Meta.RewardsDOD;
using Unity.Burst;
using Unity.Entities;

namespace TMG.Zombies
{
    [BurstCompile]
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieRiseSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            new ZombieRiseJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(ZombieRiseAspect zombie, [EntityIndexInQuery]int sortKey)
        {
            zombie.Rise(DeltaTime);
            if(!zombie.IsAboveGround) return;
            
            zombie.SetAtGroundLevel();
            ECB.RemoveComponent<ZombieRiseRate>(sortKey, zombie.Entity);
            ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, true);
            
            // [Sandbox 9] Earn 1 crystal
            var rewardsIncome = new RewardsIncome(1, RewardsKeys.Crystals);
            ECB.AddComponent(sortKey, zombie.Entity, rewardsIncome);
        }
    }

}