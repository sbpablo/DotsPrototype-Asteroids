using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class TriggerDetectionSystem : JobComponentSystem
    {

        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();
            _buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        }


        [BurstCompile]
        private struct TriggerJob : ITriggerEventsJob
        {
            public ComponentDataFromEntity<DestroyableComponent> destroyableGroup;
            [ReadOnly] public ComponentDataFromEntity<PowerUpTag> powerUpGroup;
            [ReadOnly] public ComponentDataFromEntity<InputComponent> playerGroup;

            // Improve this in next version.

            public ComponentDataFromEntity<LaserTraversalPowerUpDataComponent> laserTraversalGroup;
            public ComponentDataFromEntity<ShieldPowerUpDataComponent> shieldGroup;


            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                bool entityAIsPlayer = playerGroup.HasComponent(entityA);
                bool entityAIsPowerUp = powerUpGroup.HasComponent(entityA);

                bool entityBIsPlayer = playerGroup.HasComponent(entityB);
                bool entityBIsPowerUp = powerUpGroup.HasComponent(entityB);


                if (entityAIsPlayer && entityBIsPowerUp)
                {

                    switch (powerUpGroup[entityB].type)
                    {
                        case PowerUpType.LaserTraversal:


                            LaserTraversalPowerUpDataComponent laserTraversalPowerUp = laserTraversalGroup[entityA];
                            laserTraversalPowerUp.active = true;
                            laserTraversalGroup[entityA] = laserTraversalPowerUp;
                            break;

                        case PowerUpType.Shield:

                            ShieldPowerUpDataComponent shieldPowerUp = shieldGroup[entityA];
                            shieldPowerUp.active = true;
                            shieldGroup[entityA] = shieldPowerUp;
                            break;

                        default:
                            break;

                    }

                    DestroyableComponent entityBDestroyabe = destroyableGroup[entityB];
                    entityBDestroyabe.isDestroyed = true;
                    destroyableGroup[entityB] = entityBDestroyabe;


                }
                else if (entityAIsPowerUp && entityBIsPlayer)
                {


                    switch (powerUpGroup[entityA].type)
                    {
                        case PowerUpType.LaserTraversal:


                            LaserTraversalPowerUpDataComponent laserTraversalPowerUp = laserTraversalGroup[entityB];
                            laserTraversalPowerUp.active = true;
                            laserTraversalGroup[entityB] = laserTraversalPowerUp;
                            break;

                        case PowerUpType.Shield:

                            ShieldPowerUpDataComponent shieldPowerUp = shieldGroup[entityB];
                            shieldPowerUp.active = true;
                            shieldGroup[entityB] = shieldPowerUp;
                            break;

                        default:
                            break;

                    }

                    DestroyableComponent entityADestroyabe = destroyableGroup[entityA];
                    entityADestroyabe.isDestroyed = true;
                    destroyableGroup[entityA] = entityADestroyabe;

                }


            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var destroyable = GetComponentDataFromEntity<DestroyableComponent>(false);
            var powerUp = GetComponentDataFromEntity<PowerUpTag>(true);
            var playerShip = GetComponentDataFromEntity<InputComponent>(true);
            var laserTraversalGroupPowerUp = GetComponentDataFromEntity<LaserTraversalPowerUpDataComponent>(false);
            var shieldGroupPowerUp = GetComponentDataFromEntity<ShieldPowerUpDataComponent>(false);


            var job = new TriggerJob()
            {
                destroyableGroup = destroyable,
                powerUpGroup = powerUp,
                playerGroup = playerShip,
                laserTraversalGroup = laserTraversalGroupPowerUp,
                shieldGroup = shieldGroupPowerUp


            };


            JobHandle jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);

            jobHandle.Complete();

            return jobHandle;
        }
    }

}


