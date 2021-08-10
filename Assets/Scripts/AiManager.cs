using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class AiManager : MonoBehaviour
    {
        [SerializeField]private List<EnemyBase> enemies;
        public int EnemyCount { get; private set; }
        IEnumerator InitializeEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].OnDeath += OnEnemyDies;
                }
                yield return null;
            }
        }
        
        void OnEnemyDies()
        {
            EnemyCount--;
        }

        public void PrepareEnemies()
        {
            EnemyCount = enemies.Count;
            StartCoroutine(InitializeEnemies());
        }


    }
}