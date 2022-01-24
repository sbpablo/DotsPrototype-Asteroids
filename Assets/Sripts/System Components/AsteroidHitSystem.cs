
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class AsteroidHitSystem : SystemBase
    {

        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        protected override void OnCreate()
        {
            base.OnCreate();
            _endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }


        protected override void OnUpdate()
        {

            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            var numberOfNewAsteroids = UnityEngine.Random.Range(2, 5);


            Entities.WithAll<AsteroidTag>().ForEach((Entity entity, int entityInQueryIndex, ref AsteroidSizeAndChildDataComponent sizeAndDataComponent,
                   ref DestroyableComponent destroyableComponent, in HitComponent hitComponent, in Translation translation, in ForceComponent forceComponent) =>
            {

                if (hitComponent.wasHit == true && destroyableComponent.isDestroyed == false)
                {

                    destroyableComponent.isDestroyed = true;

                    if (sizeAndDataComponent.size == AsteroidSize.Big || sizeAndDataComponent.size == AsteroidSize.Medium)
                    {


                        for (int i = 0; i < numberOfNewAsteroids; i++)
                        {
                            var newAsteroid = ecb.Instantiate(entityInQueryIndex, sizeAndDataComponent.childAsteroidPrefab);


                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new Translation { Value = translation.Value });
                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new ForceComponent
                            { force = forceComponent.force, maxForce = forceComponent.maxForce, maxTorque = forceComponent.maxTorque, torque = forceComponent.torque });



                            ecb.SetComponent(entityInQueryIndex, newAsteroid, new MovementDirectionComponent
                            { direction = Vector3.zero }); // Not generating UnityEngine.Random.Range here because only in main thread.

                        }

                    }

                }


            }).ScheduleParallel();

            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency); ;
        }
    }



}

