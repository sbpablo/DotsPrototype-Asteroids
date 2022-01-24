using UnityEngine;
using Unity.Entities;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public class PrefabConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private GameObject _laserPrefab;
        private static Entity _laserEntityPrefab;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {


            using (var blobAssetStore = new BlobAssetStore())
            {
                Entity prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy
                                              (_laserPrefab, GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
                _laserEntityPrefab = prefabEntity;
            }

        }

        public static Entity GetLaserPrefabAsEntity()
        {
            return _laserEntityPrefab;

        }

    }

}


