using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    
    public class GameLogicManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private AiManager aiManager;

        public bool IsGameStarted { get; private set; } = false;

        private void Start()
        {
            StartGame();
        }
        public void StartGame()
        {
            IsGameStarted = true;

            if (player)
            {
                player.OnDeath += OnPlayerDies;
            }

            if (aiManager)
            {
                aiManager.PrepareEnemies();
            }
        }

        private void OnPlayerDies()
        {
            print("death");
        }
    }
}