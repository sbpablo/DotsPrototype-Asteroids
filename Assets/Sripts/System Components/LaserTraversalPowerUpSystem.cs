using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class LaserTraversalPowerUpSystem : SystemBase
    {

        private float _deltaTime;
        protected override void OnUpdate()
        {

            _deltaTime = UnityEngine.Time.deltaTime;

            Entities.WithAll<InputComponent>().ForEach((Entity entity, ref LaserTraversalPowerUpDataComponent powerUpData) =>
            {
                if (powerUpData.active)
                {
                    powerUpData.timer += _deltaTime;

                    if (powerUpData.timer < powerUpData.coolDownTime)
                    {

                        Entities.WithAll<LaserTag>().ForEach((Entity entity, ref TraverseScreenComponent traverseScreenComponent) =>
                        {

                            if (!traverseScreenComponent.shouldTraverseScreen)
                            {
                                traverseScreenComponent.shouldTraverseScreen = true;
                            }

                        }).ScheduleParallel();

                    }
                    else
                    {
                        powerUpData.active = false;
                        powerUpData.timer = 0;

                        Entities.WithAll<LaserTag>().ForEach((Entity entity, ref TraverseScreenComponent traverseScreenComponent) =>
                        {

                            if (traverseScreenComponent.shouldTraverseScreen)
                            {
                                traverseScreenComponent.shouldTraverseScreen = false;
                            }

                        }).ScheduleParallel();
                    }
                }

            }).WithoutBurst().Run();
        }
    }

}

