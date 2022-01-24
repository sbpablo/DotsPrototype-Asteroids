
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class SpaceshipMovingSystem : SystemBase
    {

        protected override void OnUpdate()
        {

            var deltaTime = Time.DeltaTime;

            Entities.WithAll<InputComponent>().ForEach((ref Translation translation, ref SpeedComponent speedComponent, ref Rotation rotation, ref InputComponent input, in HitComponent hit) =>
            {


                if (!hit.wasHit)
                {

                    translation.Value.y += input.VerticalMovementInput * deltaTime * speedComponent.Speed;
                    translation.Value.x += input.HorizontalMovementInput * deltaTime * speedComponent.Speed;


                    var currentRotation = rotation.Value;
                    var degreesToRotate = input.RotationInput * speedComponent.RotationSpeed * deltaTime;

                    var desired = math.mul(math.normalizesafe(currentRotation), quaternion.AxisAngle(math.forward(), -degreesToRotate));


                    rotation.Value = desired;
                }


            }).Run();

        }

    }

}

