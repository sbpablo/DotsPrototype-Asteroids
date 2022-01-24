
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Collections;
using System;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class CollisionDetectionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;

        public EventHandler OnPlayerHit;

        protected override void OnCreate()
        {
            base.OnCreate();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        }


        [BurstCompile]
        [UpdateAfter(typeof(EndFramePhysicsSystem))]
        struct DeathOnCollisionSystemJob : ICollisionEventsJob
        {

            [ReadOnly] public ComponentDataFromEntity<AsteroidTag> asteroidGroup;
            [ReadOnly] public ComponentDataFromEntity<LaserTag> laserGroup;
            [ReadOnly] public ComponentDataFromEntity<InputComponent> inputGroup;
            [ReadOnly] public ComponentDataFromEntity<EnemyTag> enemyGroup;
            public ComponentDataFromEntity<DestroyableComponent> destroyableGroup;
            public ComponentDataFromEntity<HitComponent> hitGroup;


            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.EntityA;
                Entity entityB = collisionEvent.EntityB;

                bool entityAIsAsteroid = asteroidGroup.HasComponent(entityA);
                bool entityAIsLaser = laserGroup.HasComponent(entityA);
                bool entityAIsPlayer = inputGroup.HasComponent(entityA);
                bool entityAIsEnemy = enemyGroup.HasComponent(entityA);



                bool entityBIsAsteroid = asteroidGroup.HasComponent(entityB);
                bool entityBIsLaser = laserGroup.HasComponent(entityB);
                bool entityBIsPlayer = inputGroup.HasComponent(entityB);
                bool entityBIsEnemy = enemyGroup.HasComponent(entityB);

              


                if (entityAIsLaser &&  (entityBIsAsteroid || entityBIsEnemy))
                {
                    DestroyableComponent entityADestroyable = destroyableGroup[entityA];
                    HitComponent entityBHitConponent = hitGroup[entityB];

                    entityADestroyable.isDestroyed = true;
                    entityBHitConponent.wasHit = true;

                    destroyableGroup[entityA] = entityADestroyable;
                    hitGroup[entityB] = entityBHitConponent;

                }


                if ( (entityAIsAsteroid || entityAIsEnemy)   && entityBIsLaser)
                {
                    DestroyableComponent entityBDestroyable = destroyableGroup[entityB];
                    HitComponent entityAHitComponent = hitGroup[entityA];

                    entityAHitComponent.wasHit = true;
                    entityBDestroyable.isDestroyed = true;

                    hitGroup[entityA] = entityAHitComponent;
                    destroyableGroup[entityB] = entityBDestroyable;

                }


                if (entityAIsAsteroid && entityBIsPlayer)
                {

                    HitComponent entityBHitComponent = hitGroup[entityB];


                    if (!entityBHitComponent.wasHit && entityBHitComponent.canBeHit)
                    {
                        entityBHitComponent.wasHit = true;
                        entityBHitComponent.canBeHit = false;

                        hitGroup[entityB] = entityBHitComponent;

                    }

                }

                if (entityAIsPlayer && entityBIsAsteroid)
                {
                    HitComponent entityAHitComponent = hitGroup[entityA];


                    if (!entityAHitComponent.wasHit && entityAHitComponent.canBeHit)
                    {
                        entityAHitComponent.wasHit = true;
                        entityAHitComponent.canBeHit = false;

                        hitGroup[entityA] = entityAHitComponent;

                    }

                }


                if (entityAIsPlayer && entityBIsEnemy)
                {
                    HitComponent entityAHitComponent = hitGroup[entityA];
                    HitComponent entityBHitComponent = hitGroup[entityB];

                    if (!entityAHitComponent.wasHit && entityAHitComponent.canBeHit)
                    {
                        entityAHitComponent.wasHit = true;
                        entityAHitComponent.canBeHit = false;

                        hitGroup[entityA] = entityAHitComponent;

                    }


                    entityBHitComponent.wasHit = true;
                    hitGroup[entityB] = entityBHitComponent;

                }


                if (entityAIsEnemy && entityBIsPlayer)
                {
                    HitComponent entityBHitComponent = hitGroup[entityB];
                    HitComponent entityAHitComponent = hitGroup[entityA];

                    if (!entityBHitComponent.wasHit && entityBHitComponent.canBeHit)
                    {
                        entityBHitComponent.wasHit = true;
                        entityBHitComponent.canBeHit = false;

                        hitGroup[entityB] = entityBHitComponent;

                    }


                    entityAHitComponent.wasHit = true;
                    hitGroup[entityA] = entityAHitComponent;

                }


            }

        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

            var destroyable = GetComponentDataFromEntity<DestroyableComponent>(false);
            var laser = GetComponentDataFromEntity<LaserTag>(true);
            var asteroid = GetComponentDataFromEntity<AsteroidTag>(true);
            var hit = GetComponentDataFromEntity<HitComponent>(false);
            var playerShip = GetComponentDataFromEntity<InputComponent>(true);
            var enemy = GetComponentDataFromEntity<EnemyTag>(true);


            var job = new DeathOnCollisionSystemJob
            {
                destroyableGroup = destroyable,
                asteroidGroup = asteroid,
                laserGroup = laser,
                hitGroup = hit,
                inputGroup = playerShip,
                enemyGroup = enemy
                

            };


            JobHandle jobHandle = job.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);

            jobHandle.Complete();

            return jobHandle;
        }
    }


}

