using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Mathematics;


namespace DotsPrototypeAsteroids.PabloSforsini
{
   
    public class LinearMovingSystem : SystemBase
    {

        protected override void OnUpdate()
        {
            Entities.WithAll<ForceComponent>().ForEach((ref ForceComponent forceComponent, ref PhysicsVelocity velocity,
                in MovementDirectionComponent movDirection, in PhysicsMass mass) =>

            {

                PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, movDirection.direction * forceComponent.force);

                if (math.length(velocity.Linear) > forceComponent.maxForce)
                    velocity.Linear = math.normalize(velocity.Linear) * forceComponent.maxForce;


            }).ScheduleParallel();
        }
    }

}

