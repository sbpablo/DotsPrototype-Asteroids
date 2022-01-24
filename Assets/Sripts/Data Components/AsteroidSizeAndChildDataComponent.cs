using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini 
{

    [GenerateAuthoringComponent]
    public struct AsteroidSizeAndChildDataComponent : IComponentData
    {
        public AsteroidSize size;
        public Entity childAsteroidPrefab;

    }
}


