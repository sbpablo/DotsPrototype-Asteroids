
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;


namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class Testing : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


            // Podría seguir agregando componentes con typeof en la misma línea de abajo, pero lo mas prolijo es armar archetypes;
            // Entity entity = entityManager.CreateEntity(typeof( SpeedComponent));

            EntityArchetype entityArchetype = entityManager.CreateArchetype
                                             (typeof(SpeedComponent), typeof(Translation), typeof(Rotation),
                                             typeof(RenderMesh), typeof(LocalToWorld),
                                             typeof(RenderBounds));

            //Como dice abajo creo uno solo, pero lo interesante es poder crear muchos, se hace con NativeArray;

            //Entity entity = entityManager.CreateEntity(entityArchetype);
            //entityManager.SetComponentData(entity,new SpeedComponent { Speed = 10, RotationSpeed = 10 } ); 

            NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);

            entityManager.CreateEntity(entityArchetype, entityArray);

            foreach (var entity in entityArray)
            {
                entityManager.SetComponentData(entity, new SpeedComponent { Speed = Random.Range(1, 10), RotationSpeed = 45 });

                entityManager.SetComponentData(entity, new LocalToWorld { });

                entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = _mesh, material = _material });


            }

            entityArray.Dispose();


        }




    }


}

