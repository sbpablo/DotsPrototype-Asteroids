using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class ChangeDirectionSystem : SystemBase
    {

        private float _deltaTime;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();


            Entities.WithAll<EnemyTag>().ForEach((Entity entity, int entityInQueryIndex, ref IndividualRandomDataComponent randomData) =>
            {
                randomData.Value = Random.CreateFromIndex((uint)entityInQueryIndex);
                randomData.hasObtainedValue = true;


            }).ScheduleParallel();


        }
        protected override void OnUpdate()
        {
            _deltaTime = UnityEngine.Time.deltaTime;

            Entities.WithAll<EnemyTag>().ForEach((Entity entity, int entityInQueryIndex, ref IndividualRandomDataComponent randomData, ref MovementDirectionComponent movementDirectionComponent, ref ChangeDirectionDataComponent change) =>
            {

                change.timer += _deltaTime;

                if (change.timer >= change.ChangeAfterXSeconds)
                {

                    if (!randomData.hasObtainedValue)
                    {
                        randomData.Value = Random.CreateFromIndex((uint)entityInQueryIndex);
                        randomData.hasObtainedValue = true;
                    }


                    movementDirectionComponent.direction = new UnityEngine.Vector3(randomData.NextValue, randomData.NextValue).normalized;
                    

                    change.timer = 0;

                }

              


            }).WithoutBurst().Run();
        }
    }
}

