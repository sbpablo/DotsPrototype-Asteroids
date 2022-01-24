
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;

namespace DotsPrototypeAsteroids.PabloSforsini
{

    public class PlayerShipEntityCreator : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;


        private void Start()
        {

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EntityArchetype entityArchetype = entityManager.CreateArchetype
                                            (typeof(SpeedComponent), typeof(Translation), typeof(Rotation),
                                            typeof(RenderMesh), typeof(LocalToWorld),
                                            typeof(RenderBounds), typeof(InputComponent), typeof(WeaponComponent));

            NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);

            entityManager.CreateEntity(entityArchetype, entityArray);

            foreach (var entity in entityArray)
            {
                entityManager.SetComponentData(entity, new SpeedComponent { Speed = _speed, RotationSpeed = _rotationSpeed });

                entityManager.SetComponentData(entity, new LocalToWorld { });

                entityManager.SetComponentData(entity, new InputComponent
                {
                    HorizontalMovementInput = 0,
                    VerticalMovementInput = 0,
                    RotationInput = 0,
                    IsFiring = false
                });


                entityManager.SetComponentData(entity, new WeaponComponent { projectilePrefab = PrefabConverter.GetLaserPrefabAsEntity(), fireRate = 0.2f });

                entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = _mesh, material = _material });


            }

            entityArray.Dispose();

        }


    }

}
