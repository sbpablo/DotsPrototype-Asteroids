using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class ShieldSystem : SystemBase
    {
        private float _deltaTime;
        protected override void OnUpdate()
        {
            _deltaTime = UnityEngine.Time.deltaTime;

            Entities.WithAll<ShieldPowerUpDataComponent>().ForEach((ref ShieldPowerUpDataComponent shield, ref HitComponent hit) =>
            {
                if (shield.active)
                {
                    if (hit.canBeHit)
                    {
                        hit.canBeHit = false;
                    }

                    shield.timer += _deltaTime;

                    if (shield.timer >= shield.coolDownTime)
                    {
                        shield.active = false;
                        hit.canBeHit = true;
                        shield.timer = 0;
                    }
                }


            }).WithoutBurst().Run();

        }
    }


}
