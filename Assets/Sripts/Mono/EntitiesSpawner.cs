using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

namespace DotsPrototypeAsteroids.PabloSforsini
{
    public enum AsteroidSize
    {
        Big,
        Medium,
        Small

    }
    public class EntitiesSpawner : MonoBehaviour
    {

        [Header("Asteroid Settings")]
        [SerializeField] private List<GameObject> _asteroidPrefabs;
        [SerializeField] private int _asteroidSpawnCount;
        [SerializeField] private int _asteroidSpawnDelay;

        [Header("PowerUp Settings")]
        [SerializeField] private List<GameObject> _powerUpPrefabs;
        [SerializeField] private int _powerUpSpawnCount;
        [SerializeField] private int _powerUpSpawnDelay;

        [Header("Enemies Settings")]
        [SerializeField] private List<GameObject> _enemiesPrefabs;
        [SerializeField] private int _enemiesSpawnCount;
        [SerializeField] private int _enemiesSpawnDelay;


        [Header("SpawnPoints Settings")]
        [SerializeField] private Transform[] _spawnPoints;

        private static EntitiesSpawner _instance;

        public static EntitiesSpawner Instance { get { return _instance; } }


        private EntityManager _entityManager;
        private List<Entity> _asteroidEntityPrefabs = new List<Entity>();
        private List<Entity> _powerUpEntityPrefabs = new List<Entity>();
        private List<Entity> _enemyEntityPrefabs = new List<Entity>();

        private List<BlobAssetStore> _asteroidBlobAssetStoreList = new List<BlobAssetStore>();
        private List<BlobAssetStore> _powerUpBlobAssetStoreList = new List<BlobAssetStore>();
        private List<BlobAssetStore> _enemyBlobAssetStoreList = new List<BlobAssetStore>();

        private WaitForSeconds _asteroidSpawnTime;
        private WaitForSeconds _powerUpSpawnTime;
        private WaitForSeconds _enemySpawnTime; 

        private void Awake()
        {
           

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            ConvertGOsToEntities(_asteroidPrefabs, _asteroidBlobAssetStoreList, _asteroidEntityPrefabs);
            ConvertGOsToEntities(_powerUpPrefabs, _powerUpBlobAssetStoreList, _powerUpEntityPrefabs);
            ConvertGOsToEntities(_enemiesPrefabs, _enemyBlobAssetStoreList, _enemyEntityPrefabs);

        }


        private void OnEnable()
        {
           GameManager.Instance.OnGameOver += StopAllCoroutines;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameOver -= StopAllCoroutines;
        }


        void Start()
        {

            _asteroidSpawnTime = new WaitForSeconds(_asteroidSpawnDelay);
            _powerUpSpawnTime = new WaitForSeconds(_powerUpSpawnDelay);
            _enemySpawnTime = new WaitForSeconds(_enemiesSpawnDelay);

            StartCoroutine(SpawnAsteroids());
            StartCoroutine(SpawnPowerUps());
            StartCoroutine(SpawnEnemies());


        }

        private void ConvertGOsToEntities(List<GameObject> goList, List<BlobAssetStore> blobAssetStoreList, List<Entity> entityList)
        {

            for (int i = 0; i < goList.Count; i++)
            {
                blobAssetStoreList.Add(new BlobAssetStore());
                var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStoreList[i]);
                entityList.Add(GameObjectConversionUtility.ConvertGameObjectHierarchy(goList[i], settings));
            }
        }

        private IEnumerator SpawnPowerUps()
        {

            while (true)  // Set conditions later
            {

                yield return _powerUpSpawnTime;

                NativeArray<Entity> powerUpArray = new NativeArray<Entity>(_powerUpSpawnCount, Allocator.Temp);


                for (int i = 0; i < powerUpArray.Length; i++)
                {
                    powerUpArray[i] = _entityManager.Instantiate(_powerUpEntityPrefabs[UnityEngine.Random.Range(0, _powerUpEntityPrefabs.Count)]);

                    var translation = new Translation
                    { Value = GetRandomPointInsideScreen() };

                    _entityManager.SetComponentData(powerUpArray[i], translation);

                }

                powerUpArray.Dispose();

            }

        }
        private IEnumerator SpawnAsteroids()
        {

            while (true)  // Set conditions later
            {

                NativeArray<Entity> asteroidArray = new NativeArray<Entity>(_asteroidSpawnCount, Allocator.Temp);


                for (int i = 0; i < asteroidArray.Length; i++)
                {
                    asteroidArray[i] = _entityManager.Instantiate(_asteroidEntityPrefabs[0]);


                    var force = new ForceComponent
                    { force = 0.1f, torque = 1, maxForce = UnityEngine.Random.Range(0.1f, 2f), maxTorque = UnityEngine.Random.Range(-3f, 3f) };

                    _entityManager.SetComponentData(asteroidArray[i], force);

                    var translation = new Translation
                    { Value = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].transform.position };

                    _entityManager.SetComponentData(asteroidArray[i], translation);


                    var traverseDirection = GetRandomPointInsideScreen() - (Vector3)_entityManager.GetComponentData<Translation>(asteroidArray[i]).Value;

                    _entityManager.SetComponentData(asteroidArray[i], new MovementDirectionComponent { direction = traverseDirection.normalized });


                }

                asteroidArray.Dispose();

                yield return _asteroidSpawnTime;

            }

        }


        private IEnumerator SpawnEnemies()
        {

            while (true)  // Set conditions later
            {

                yield return _enemySpawnTime;

                NativeArray<Entity> enemyArray = new NativeArray<Entity>(_enemiesSpawnCount, Allocator.Temp);


                for (int i = 0; i < enemyArray.Length; i++)
                {
                    enemyArray[i] = _entityManager.Instantiate(_enemyEntityPrefabs[UnityEngine.Random.Range(0, _enemyEntityPrefabs.Count)]);


                    var translation = new Translation
                    { Value = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].transform.position };

                    _entityManager.SetComponentData(enemyArray[i], translation);

                    var traverseDirection = GetRandomPointInsideScreen() - (Vector3)_entityManager.GetComponentData<Translation>(enemyArray[i]).Value;

                    _entityManager.SetComponentData(enemyArray[i], new MovementDirectionComponent { direction = traverseDirection.normalized });


                }

                enemyArray.Dispose();

            }

        }


        /*
        public void SpawnAsteroids(int count, Translation translation, AsteroidSize size)
        {
            NativeArray<Entity> asteroidArray = new NativeArray<Entity>(count, Allocator.Temp);

            Entity entityToSpawn = _asteroidEntityPrefabs[_asteroidEntityPrefabs.Count-1];  //Default the smallest, the latest in the list;

            for (int i=0; i< _asteroidEntityPrefabs.Count; i++)
            {
                var component = _entityManager.GetComponentData<AsteroidSizeAndChildDataComponent>(_asteroidEntityPrefabs[i]);

                if (component.size == size)
                {
                    entityToSpawn = _asteroidEntityPrefabs[i];
                    break;
                }


            }



            for (int i = 0; i < asteroidArray.Length; i++)
            {
                asteroidArray[i] = _entityManager.Instantiate(entityToSpawn);

                //_entityManager.SetComponentData(asteroidArray[i], new Translation { Value = Vector3.zero });

                var force = new ForceComponent
                { force = 0.1f, torque = 1, maxForce = UnityEngine.Random.Range(0.1f, 2f), maxTorque = UnityEngine.Random.Range(-3f, 3f) };

                _entityManager.SetComponentData(asteroidArray[i], force);


                _entityManager.SetComponentData(asteroidArray[i], translation);


                var traverseDirection = GetRandomPointInsideScreen() - (Vector3)_entityManager.GetComponentData<Translation>(asteroidArray[i]).Value;

                _entityManager.SetComponentData(asteroidArray[i], new MovementDirectionComponent { direction = traverseDirection.normalized });




            }
        } */

        private void OnDestroy()
        {

            DisposeBlobAssetList(_asteroidBlobAssetStoreList);
            DisposeBlobAssetList(_enemyBlobAssetStoreList);
            DisposeBlobAssetList(_powerUpBlobAssetStoreList);
            
           
        }

        private Vector3 GetRandomPointInsideScreen()
        {

            var randomX = UnityEngine.Random.Range(ScreenInfo.GetScreenBottomCorner().x, ScreenInfo.GetScreenTopCorner().x);
            var randomY = UnityEngine.Random.Range(ScreenInfo.GetScreenBottomCorner().y, ScreenInfo.GetScreenTopCorner().y);

            return new Vector3(randomX, randomY);

        }

        private void DisposeBlobAssetList ( List<BlobAssetStore> list)
        {

            for (int i=0; i<list.Count; i++)
            {
                list[i].Dispose();
            }

        }
    }

}
