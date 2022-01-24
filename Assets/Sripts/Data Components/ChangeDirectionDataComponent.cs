using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct ChangeDirectionDataComponent : IComponentData
    {

        public float ChangeAfterXSeconds;
        public float timer;

}

}
