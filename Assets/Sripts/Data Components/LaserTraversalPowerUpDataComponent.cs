using Unity.Entities;


namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct LaserTraversalPowerUpDataComponent : IComponentData
    {
        public float timer;
        public float coolDownTime;
        public bool active;

    }

}

