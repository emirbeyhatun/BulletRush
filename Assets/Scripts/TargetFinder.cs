using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class TargetFinder : MonoBehaviour
    {
        private Dictionary<int, Enemy> enemiesInRange = new Dictionary<int, Enemy>();
        private Enemy currentTarget;
        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy && enemiesInRange != null)
            {
                if (enemiesInRange.ContainsKey(enemy.gameObject.GetInstanceID()) == false)
                {
                    enemiesInRange.Add(enemy.gameObject.GetInstanceID(), enemy);
                    OnEnemyOrderChanges();
                }
            }
        }

        private void Update()
        {
            if(currentTarget == null)
            {
                CalculateCurrentTarget();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy && enemiesInRange != null)
            {
                if (enemiesInRange.ContainsKey(enemy.gameObject.GetInstanceID()) == true)
                {
                    enemiesInRange.Remove(enemy.gameObject.GetInstanceID());
                    OnEnemyOrderChanges();
                }
            }
        }

        public void RemoveEnemyIfDead(Enemy enemy)
        {
            if(enemiesInRange != null && enemy.IsDead())
            {
                int instanceID = enemy.gameObject.GetInstanceID();
                if (enemiesInRange.ContainsKey(instanceID))
                {
                    enemiesInRange.Remove(instanceID);
                }

                if(currentTarget == enemy)
                {
                    currentTarget = null;
                }
            }
        }

        public void OnEnemyOrderChanges()
        {
            CalculateCurrentTarget();
        }

        private void CalculateCurrentTarget()
        {
            if (enemiesInRange == null) return;

            Enemy closest = null;

            foreach (KeyValuePair<int, Enemy> pair in enemiesInRange)
            {
                if(closest == null && pair.Value)
                {
                    closest = pair.Value;
                }
                else if(closest)
                {
                    if(Vector3.Distance(closest.transform.position, transform.position) > Vector3.Distance(pair.Value.transform.position, transform.position))
                    {
                        closest = pair.Value;
                    }
                }
            }

            currentTarget = closest;
        }

        public Enemy GetTarget()
        {
            return currentTarget;
        }


    }
}