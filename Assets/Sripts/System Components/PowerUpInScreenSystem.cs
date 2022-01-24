using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class PowerUpInScreenSystem : SystemBase
    {
        private float _deltaTime;


        protected override void OnUpdate()
        {
            _deltaTime = UnityEngine.Time.deltaTime;

            Entities.WithAll<PowerUpTag>().ForEach((ref PowerUpTag powerUp, ref DestroyableComponent destroyable) =>
            {

                powerUp.timer += _deltaTime;

                if (powerUp.timer >= powerUp.timeInScreen)
                {
                    destroyable.isDestroyed = true;
                }


            }).WithoutBurst().Run();

        }
    }
}

