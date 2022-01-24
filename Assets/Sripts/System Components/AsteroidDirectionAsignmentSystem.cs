using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace DotsPrototypeAsteroids.PabloSforsini
{

    public class AsteroidDirectionAsignmentSystem : SystemBase
    {

        protected override void OnStartRunning()
        {
            base.OnStartRunning();


            Entities.WithAll<AsteroidTag>().ForEach((Entity entity, int entityInQueryIndex, ref IndividualRandomDataComponent randomData) =>
            {
                randomData.Value = Random.CreateFromIndex((uint)entityInQueryIndex);
                randomData.hasObtainedValue = true;


            }).ScheduleParallel();


        }

        protected override void OnUpdate()
        {



            Entities.WithAll<AsteroidTag>().ForEach((Entity entity, int entityInQueryIndex, ref IndividualRandomDataComponent randomData, ref MovementDirectionComponent movementDirectionComponent) =>
            {
                if (!randomData.hasObtainedValue)
                {
                    randomData.Value = Random.CreateFromIndex((uint)entityInQueryIndex);
                    randomData.hasObtainedValue = true;
                }


                if (movementDirectionComponent.direction == UnityEngine.Vector3.zero)
                {

                    movementDirectionComponent.direction = new UnityEngine.Vector3(randomData.NextValue, randomData.NextValue).normalized;
                }


            }).ScheduleParallel();





        }

    }
}

   
