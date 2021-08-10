using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class TargetFinder : MonoBehaviour
    {
        private Dictionary<int, EnemyBase> enemiesInRange = new Dictionary<int, EnemyBase>();
        private EnemyBase currentTarget;
        private void OnTriggerEnter(Collider other)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy && enemiesInRange != null)
            {
                if (enemiesInRange.ContainsKey(enemy.gameObject.GetInstanceID()) == false)
                {
                    enemiesInRange.Add(enemy.gameObject.GetInstanceID(), enemy);
                    OnEnemyOrderChanges();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy && enemiesInRange != null)
            {
                if (enemiesInRange.ContainsKey(enemy.gameObject.GetInstanceID()) == true)
                {
                    enemiesInRange.Remove(enemy.gameObject.GetInstanceID());
                    OnEnemyOrderChanges();
                }
            }
        }

        public void RemoveEnemyIfDead(EnemyBase enemy)
        {
            if(enemiesInRange != null && enemy.IsDead())
            {
                int instanceID = enemy.GetInstanceID();
                if (enemiesInRange.ContainsKey(instanceID))
                {
                    enemiesInRange.Remove(instanceID);
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

            EnemyBase closest = null;

            foreach (KeyValuePair<int, EnemyBase> pair in enemiesInRange)
            {
                if(closest == null && pair.Value)
                {
                    closest = pair.Value;
                }
                else
                {
                    if(Vector3.Distance(closest.transform.position, transform.position) > Vector3.Distance(pair.Value.transform.position, transform.position))
                    {
                        closest = pair.Value;
                    }
                }
            }

            currentTarget = closest;
        }

        public EnemyBase GetTarget()
        {
            return currentTarget;
        }


    }
}