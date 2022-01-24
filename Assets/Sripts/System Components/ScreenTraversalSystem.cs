using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class ScreenTraversalSystem : SystemBase
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

            Entities.WithAll<TraverseScreenComponent>().ForEach((Entity entity, ref Translation trans, ref DestroyableComponent destroyable, in TraverseScreenComponent traverse) =>
            {

                if (traverse.shouldTraverseScreen)
                {
                    if (trans.Value.x < _bottomCorner.x)
                    {
                        trans.Value.x = _topCorner.x;
                    }
                    else if (trans.Value.x > _topCorner.x)
                    {
                        trans.Value.x = _bottomCorner.x;
                    }


                    if (trans.Value.y > _topCorner.y)
                    {
                        trans.Value.y = _bottomCorner.y;
                    }
                    else if (trans.Value.y < _bottomCorner.y)
                    {
                        trans.Value.y = _topCorner.y;
                    }
                }
                else
                {
                    if (trans.Value.x < _bottomCorner.x)
                    {
                        destroyable.isDestroyed = true;
                    }
                    else if (trans.Value.x > _topCorner.x)
                    {
                        destroyable.isDestroyed = true;
                    }


                    if (trans.Value.y > _topCorner.y)
                    {
                        destroyable.isDestroyed = true;
                    }
                    else if (trans.Value.y < _bottomCorner.y)
                    {
                        destroyable.isDestroyed = true;
                    }
                }


            }).WithoutBurst().Run();

        }
    }

}

