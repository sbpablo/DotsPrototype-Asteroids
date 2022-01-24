using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct IndividualRandomDataComponent : IComponentData
    {
        public Random Value;
        public bool hasObtainedValue;

        public float minimum;
        public float maximum;

        public float NextValue => Value.NextFloat(minimum, maximum);

    }


}

