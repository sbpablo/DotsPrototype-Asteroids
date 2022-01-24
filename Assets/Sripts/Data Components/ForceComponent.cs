
using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    [GenerateAuthoringComponent]
    public struct ForceComponent : IComponentData
    {
        public float torque;   // Positive -> anti-clockwise direction   Negative -> clockwise direction
        public float force;

        public float maxForce;
        public float maxTorque;

    }

}


