using UnityEngine;
using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class InputSystem : SystemBase
    {

        private string _horizontalAxisName = "Horizontal";
        private string _verticalAxisName = "Vertical";
        private string _mouseXAxisName = "Mouse X";
        protected override void OnUpdate()
        {
            Entities.ForEach((ref InputComponent input) =>
            {
                input.VerticalMovementInput = Input.GetAxis(_verticalAxisName);
                input.HorizontalMovementInput = Input.GetAxis(_horizontalAxisName);
                input.IsFiring = Input.GetMouseButtonDown(0);
                input.RotationInput = Input.GetAxis(_mouseXAxisName);
                input.IsHyperSpace = Input.GetMouseButtonDown(1);


            }).WithoutBurst().Run();
        }
    }

}

