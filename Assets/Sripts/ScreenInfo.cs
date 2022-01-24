using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public static class ScreenInfo
    {
        private static float _mainCameraDistance = 10;



        public static Vector3 GetScreenBottomCorner()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, _mainCameraDistance));
        }

        public static Vector3 GetScreenTopCorner()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, _mainCameraDistance));
        }

        public static float GetMainCameraDistance()
        {
            return _mainCameraDistance;
        }





    }


}



