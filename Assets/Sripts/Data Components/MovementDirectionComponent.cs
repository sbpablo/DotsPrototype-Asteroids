
using UnityEngine;
using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{

    [GenerateAuthoringComponent]
    public struct MovementDirectionComponent : IComponentData
    {
        public Vector3 direction;

     
        
    }

}

