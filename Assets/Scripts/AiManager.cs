using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletRushGame
{
    public class AiManager : MonoBehaviour
    {
        private List<Enemy> enemies = new List<Enemy>();
        public int EnemyCount { get; private set; }
        public Action OnAllEnemiesDie{ get; set; }

        public Transform enemySpawnObject;
        public Text enemyRemainingCount;
        private const string remainingEnemyString = "Enemies Left : {0}";


        //Find Enemies in the Scene inside the enemySpawnObject
        public void CollectEnemiesInScene()
        {
            if (enemySpawnObject && enemies != null)
            {
                for (int i = 0; i < enemySpawnObject.childCount; i++)
                {
                    if (enemySpawnObject.GetChild(i))
                    {
                        Enemy enemy = enemySpawnObject.GetChild(i).GetComponent<Enemy>();
                        if (enemy && enemies.Contains(enemy) == false)
                        {
                            enemies.Add(enemy);
                        }
                    }
                }
            }
        }

        private IEnumerator InitializeEnemies(Transform target)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].SetTarget(target);
                    enemies[i].OnDeath += OnEnemyDies;
                    enemies[i].StartAttacking();
                }
                yield return null;
            }
        }
        
        void OnEnemyDies()
        {
            EnemyCount--;

            UpdateEnemyReaminingText(EnemyCount);

            if (EnemyCount <= 0)
            {
                OnAllEnemiesDie?.Invoke();
            }
        }

        public void PrepareAndInitEnemies(Transform target)
        {
            EnemyCount = enemies.Count;
            UpdateEnemyReaminingText(EnemyCount);

            StartCoroutine(InitializeEnemies(target));
        }

        public void UpdateEnemyReaminingText(int remaining)
        {
            if (enemyRemainingCount)
            {
                enemyRemainingCount.text = string.Format(remainingEnemyString, remaining);
            }
        }

    }
}