using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class DestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithoutBurst().WithStructuralChanges().WithAll<DestroyableComponent>().ForEach((Entity _entity, in DestroyableComponent destroyable) =>
            {
                if (destroyable.isDestroyed)
                {
                    EntityManager.DestroyEntity(_entity);

                }


            }).Run();
        }
    }


}
