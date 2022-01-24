using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Entities;
using UnityEngine.UI;

namespace DotsPrototypeAsteroids.PabloSforsini
{

    public class UIManager : MonoBehaviour
    {

        [SerializeField] private TMP_Text _livesText;
        [SerializeField] private Button _restartButton;
        private string _livesString = "Lives: ";


        private static UIManager _instance;

        public static UIManager Instance
        {
            get { return _instance; }
        }
        private void Awake()
        {

            if (_instance != null && _instance != this.gameObject)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
              
            }

        }      

        private void OnEnable()
        {

            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PlayerShipHitSystem>().OnLifeLost += UIManager_OnLifeLost;
           GameManager.Instance.OnGameOver += EnableRestartButton;

        }
        private void OnDisable()
        {
            // World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PlayerShipHitSystem>().OnLifeLost -= UIManager_OnLifeLost;
            GameManager.Instance.OnGameOver -= EnableRestartButton;

        }

        private void Start()
        {
           
            _restartButton.gameObject.SetActive(false);
        }

        private void EnableRestartButton()
        {
            _restartButton.gameObject.SetActive(true);
        }

       

        private void UIManager_OnLifeLost(int lives)
        {
            _livesText.text = _livesString + lives.ToString();
        }
    }


}
