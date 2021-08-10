using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletRushGame
{
    
    public class GameLogicManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private AiManager aiManager;


        public GameObject winScreen;
        public GameObject loseScreen;


        private void Start()
        {
            StartGame();
        }
        public void StartGame()
        {

            if (player)
            {
                player.OnDeath += OnPlayerDies;
            }

            if (aiManager)
            {
                aiManager.CollectEnemiesInScene();
                aiManager.OnAllEnemiesDie = OpenWinScreen;
                aiManager.PrepareAndInitEnemies(player.transform);
            }
        }

        private void OnPlayerDies()
        {
            OpenLoseScreen();
        }

        public void OpenWinScreen()
        {
            Time.timeScale = 0;

            if (winScreen)
            {
                winScreen.gameObject.SetActive(true);
            }
        }
        public void OpenLoseScreen()
        {
            Time.timeScale = 0;

            if (loseScreen)
            {
                loseScreen.gameObject.SetActive(true);
            }
        }

        public void ResetLevel()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}