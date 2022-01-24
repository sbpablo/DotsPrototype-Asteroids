using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct HitComponent : IComponentData
    {
        public bool wasHit; public bool canBeHit; public float respawnTime; public float timer;
    }
}

