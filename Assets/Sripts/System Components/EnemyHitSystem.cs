using Unity.Entities;


namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class EnemyHitSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<EnemyTag>().ForEach ( ( ref DestroyableComponent destroyable, in HitComponent hit) =>
           {
               if (hit.wasHit)
               {
                   destroyable.isDestroyed = true;
               }

           }).ScheduleParallel();
        }
    }

}

