using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;
using System;

namespace DotsPrototypeAsteroids.PabloSforsini
{

    public enum GameState
    {
        Running,
        GameOver
    }
    public class GameManager : MonoBehaviour
    {

        public event Action OnGameOver;

        private static GameManager _instance;

        public static GameManager Instance
        {
            get { return _instance; }
        }

        private GameState _gameState;
        public GameState GameState => _gameState;

        private void Awake()
        {

            if (_instance != null && _instance != this.gameObject)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                //DontDestroyOnLoad(this.gameObject);

            }

        }


        private void OnEnable()
        {

            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PlayerShipHitSystem>().OnAllLivesLost += SetGameOVer;
            SceneManager.sceneLoaded += SetRunningGame;

        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SetRunningGame;
           // World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PlayerShipHitSystem>().OnAllLivesLost -= SetGameOVer;
        }

        private void SetRunningGame(Scene scene, LoadSceneMode loadMode)
        {
            _gameState = GameState.Running;
        }

        private void SetGameOVer( )
        {
            _gameState = GameState.GameOver;
            OnGameOver?.Invoke();
        }

        public void GameRestart()
        {

            StartCoroutine(CloseWorldEntities());

        }

        IEnumerator CloseWorldEntities()
        {

            yield return new WaitForSeconds(1.0f);


            
            var entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            
            
            entityManager.DestroyEntity(entityManager.UniversalQuery);
            

         

            
            var ao= SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

            while (!ao.isDone)
            {
                yield return null;
            } 

        }



    }


}
