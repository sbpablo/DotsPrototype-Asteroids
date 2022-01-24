using Unity.Entities;


namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct SpeedComponent : IComponentData
    {
        public float Speed; public float RotationSpeed;
    }
}
