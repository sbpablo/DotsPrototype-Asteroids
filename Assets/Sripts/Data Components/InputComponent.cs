
using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{

    [GenerateAuthoringComponent]
    public struct InputComponent : IComponentData
    {
        private float _horizontalMovementInput;
        private float _verticalMovementInput;
        private float _rotationInput;
        private bool _isFiring;
        private bool _isHyperSpace;


        public float HorizontalMovementInput { get { return _horizontalMovementInput; } set { _horizontalMovementInput = value; } }

        public float VerticalMovementInput { get { return _verticalMovementInput; } set { _verticalMovementInput = value; } }


        public float RotationInput { get { return _rotationInput; } set { _rotationInput = value; } }

        public bool IsFiring { get { return _isFiring; } set { _isFiring = value; } }

        public bool IsHyperSpace { get { return _isHyperSpace; } set { _isHyperSpace = value; } }


    }

}

