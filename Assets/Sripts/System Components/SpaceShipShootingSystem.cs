using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class SpaceShipShootingSystem : SystemBase
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

            Entities.WithAll<InputComponent>().ForEach((int entityInQueryIndex, ref WeaponComponent weapon, ref Translation trans, ref Rotation rot, ref InputComponent input, ref HitComponent hit) =>
            {


                if (input.IsFiring && !hit.wasHit)
                {

                    var laser = ecb.Instantiate(entityInQueryIndex, weapon.projectilePrefab);

                    ecb.SetComponent(entityInQueryIndex, laser, new Rotation { Value = rot.Value });

                    ecb.SetComponent(entityInQueryIndex, laser, new Translation { Value = trans.Value });

                    var forceDirection = math.mul(rot.Value, math.up());

                    ecb.SetComponent(entityInQueryIndex, laser, new MovementDirectionComponent { direction = ((Vector3)forceDirection).normalized });

                }


            }).ScheduleParallel();

            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);

        }
    }

}


