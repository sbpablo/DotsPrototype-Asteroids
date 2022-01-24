using Unity.Entities;


namespace DotsPrototypeAsteroids.PabloSforsini
{
    public enum PowerUpType
    {
        LaserTraversal,
        Shield
    }

    [GenerateAuthoringComponent]
    public struct PowerUpTag : IComponentData
    {

        public PowerUpType type;
        public float timeInScreen;
        public float timer;

    }

}

