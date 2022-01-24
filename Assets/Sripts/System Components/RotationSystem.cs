using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Mathematics;


namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class RotationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<ForceComponent>().ForEach((ref ForceComponent forceComponent, ref PhysicsVelocity velocity, ref Rotation rot, in PhysicsMass mass) =>
            {

                PhysicsComponentExtensions.ApplyAngularImpulse(ref velocity, mass, Vector3.forward * forceComponent.torque);

                var currentAngularVelocity = PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in velocity, in mass, in rot);

                var torqueSign = Mathf.Sign(forceComponent.torque);


                if (math.length(currentAngularVelocity) > forceComponent.maxTorque)
                {

                    PhysicsComponentExtensions.SetAngularVelocityWorldSpace(ref velocity, mass, rot,
                        math.normalize(currentAngularVelocity) * forceComponent.maxTorque);
                }


            }).Run();
        }
    }

}

