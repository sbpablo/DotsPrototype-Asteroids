
using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct DestroyableComponent : IComponentData
    {

        public bool isDestroyed;

    }


}

