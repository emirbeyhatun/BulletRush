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
        public int damage = 100;
        public bool isDead = false;
    }
    public class Enemy : MonoBehaviour, IDamagable
    {
        private NavMeshAgent navmeshAgent;
        private bool startFollowing = false;
        private Transform targetToMove;
        [SerializeField]private EnemyStats stats;

        public event Action OnDeath;


        private void Awake()
        {
            navmeshAgent = GetComponent<NavMeshAgent>();
        }

        public virtual void Start()
        {
            
        }


        public void StartAttacking()
        {
            startFollowing = true;
        }

        public virtual void Update()
        {
            if (startFollowing && targetToMove && navmeshAgent)
            {
                navmeshAgent.SetDestination(targetToMove.position);
            }
        }
        

        public void SetStats(EnemyStats stats)
        {
            this.stats = stats;
        }

        public bool TakeDamage(int damage, out bool isDead)
        {
            isDead = false;
            if (stats == null) return false;
            if (stats.isDead == true) return false;

            stats.hp -= damage;
            stats.hp = Mathf.Max(0, stats.hp);
            if (stats.hp == 0)
            {
                stats.isDead = true;
                isDead = true; 

                OnDeath?.Invoke();

                gameObject.SetActive(false);
                Destroy(gameObject, 5);
            }

            return true;
        }

        internal void SetTarget(Transform target)
        {
            targetToMove = target;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                IDamagable damagable = player;
                if (damagable != null && stats != null)
                {
                    bool isDead;
                    damagable.TakeDamage(stats.damage, out isDead);
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