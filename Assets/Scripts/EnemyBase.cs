using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BulletRushGame
{
    [System.Serializable]
    public class EnemyStats
    {
        public int hp = 100;
        public int speed = 100;
        public int damage = 20;
        public bool isDead = false;
    }
    public class EnemyBase : MonoBehaviour, IDamagable
    {
        private NavMeshAgent navmeshAgent;
        [SerializeField]private EnemyStats stats;
        [SerializeField]private Transform targetToMove;

        public event Action OnDeath;

        private void Awake()
        {
            navmeshAgent = GetComponent<NavMeshAgent>();
        }

        public virtual void Start()
        {
            
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (targetToMove && navmeshAgent)
                {
                    print(navmeshAgent.SetDestination(targetToMove.position));
                }
            }
        }

        public void SetStats(EnemyStats stats)
        {
            this.stats = stats;
        }

        public bool TakeDamage(int damage)
        {
            if (stats == null) return false;
            if (stats.isDead == true) return false;

            stats.hp -= damage;
            stats.hp = Mathf.Max(0, stats.hp);
            if (stats.hp == 0)
            {
                stats.isDead = true;
                OnDeath?.Invoke();

                gameObject.SetActive(false);
                Destroy(gameObject, 1);
            }

            return true;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                IDamagable damagable = player;
                if (damagable != null && stats != null)
                {
                    damagable.TakeDamage(stats.damage);
                }
            }
        }

        public bool IsDead()
        {
            if (stats == null) return true;

            return stats.isDead;
        }
    }
}