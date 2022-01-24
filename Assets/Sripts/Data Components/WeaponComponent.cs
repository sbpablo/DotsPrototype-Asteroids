using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct WeaponComponent : IComponentData
    {
        public Entity projectilePrefab; public float fireRate;
    }
}
    

