using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct TraverseScreenComponent : IComponentData
    {
        public bool shouldTraverseScreen;
    }
}
