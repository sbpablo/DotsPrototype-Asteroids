using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using System;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class PlayerShipHitSystem : SystemBase
    {

        private float _deltaTime;

        public event Action<int> OnLifeLost;

        public event Action OnAllLivesLost;
        public struct LifeLostEvent
        {
            public int currentLives;
        }

        private NativeQueue<LifeLostEvent> _eventQueue;

        protected override void OnCreate()
        {
            base.OnCreate();
            _eventQueue = new NativeQueue<LifeLostEvent>(Allocator.Persistent);
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _eventQueue.Dispose();
        }


        // This system may need to be updated.   It could spawn in the center when no asteroids are in place and should avoid duplicate 
        // collisions.     A basic solution was to introduce a  no damage  time frame after being hit.
        protected override void OnUpdate()
        {

            _deltaTime = UnityEngine.Time.deltaTime;

            NativeQueue<LifeLostEvent>.ParallelWriter eventQueueParallel = _eventQueue.AsParallelWriter();





            Entities.WithAll<InputComponent>().ForEach((Entity entity, ref HitComponent hitComponent, ref LivesComponent lives,
                                    ref Translation trans, ref Rotation rot, ref DestroyableComponent destroyable, ref ShieldPowerUpDataComponent shield, ref InputComponent input) =>
            {

                if (hitComponent.wasHit && !shield.active)
                {


                    lives.lives--;
                    eventQueueParallel.Enqueue(new LifeLostEvent { currentLives=lives.lives });


                    if (lives.lives > 0)
                    {

                       // Debug.Log($"PLAYER HIT !!!!!!!!!!!!   CURRENT LIVES: {lives.lives }");

                        trans.Value = Vector3.zero;
                        rot.Value = Quaternion.identity;
                        hitComponent.wasHit = false;
                    }
                    else
                    {
                        destroyable.isDestroyed = true;
                        // Debug.Log($"GAME OVER");
                    }

                }

            }).ScheduleParallel();

            Dependency.Complete();

            while (_eventQueue.TryDequeue(out LifeLostEvent lifeLostEvent))
            {
                OnLifeLost?.Invoke(lifeLostEvent.currentLives);

                if (lifeLostEvent.currentLives == 0)
                {
                    OnAllLivesLost?.Invoke();
                }
            }

        }
    }

}

