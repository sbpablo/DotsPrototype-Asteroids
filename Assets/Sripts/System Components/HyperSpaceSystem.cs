
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class HyperSpaceSystem : SystemBase
    {

        private Vector3 _bottomCorner;
        private Vector3 _topCorner;


        protected override void OnStartRunning()
        {
            _bottomCorner = ScreenInfo.GetScreenBottomCorner();
            _topCorner = ScreenInfo.GetScreenTopCorner();

        }
        protected override void OnUpdate()
        {


            Entities.WithAll<InputComponent>().ForEach((ref Translation translation, in InputComponent input, in HitComponent hit) =>

            {

                if (input.IsHyperSpace && !hit.wasHit)
                {
                    // Dont forget to analize if accessing static data can generate issues, why and how.

                    var randomX = UnityEngine.Random.Range(ScreenInfo.GetScreenBottomCorner().x, ScreenInfo.GetScreenTopCorner().x);
                    var randomY = UnityEngine.Random.Range(ScreenInfo.GetScreenBottomCorner().y, ScreenInfo.GetScreenTopCorner().y);

                    translation.Value = new Vector3(randomX, randomY);

                }


            }).WithoutBurst().Run();
        }
    }


}
