using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class Bullet : MonoBehaviour
    {
        private int damage = 100;
        private Rigidbody rb;
        private Collider cl;

        private MeshRenderer meshRenderer;
        public Action<Bullet> OnBulletDies { get; set; }
        public Action<Enemy> OnEnemyDies { get;  set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cl = GetComponent<Collider>();
            meshRenderer = GetComponent<MeshRenderer>();
        }
        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                bool isDead = false;
                enemy.TakeDamage(damage, out isDead);
                if (isDead)
                {
                    OnEnemyDies?.Invoke(enemy);
                }

                DisableBullet();
                StartCoroutine(PoolAfterSomeTime(2));
            }
        }

        public void Launch(Vector3 dir, int damage, float bulletSpeed)
        {
            if (rb)
            {
                EnableBullet();
                StartCoroutine(PoolAfterSomeTime(10));
                SetDamage(damage);
                rb.velocity = dir * bulletSpeed;
            }
        }

        public void EnableBullet()
        {
            if (meshRenderer && cl)
            {
                cl.enabled = true;
                meshRenderer.enabled = true;
                StopAllCoroutines();
                gameObject.SetActive(true);
            }
        }

        public void DisableBullet()
        {
            if (meshRenderer && cl)
            {
                cl.enabled = false;
                meshRenderer.enabled = false;
                if (rb)
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
        IEnumerator PoolAfterSomeTime(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            PoolBullet();
        }

        public void PoolBullet()
        {
            gameObject.SetActive(false);

            if (OnBulletDies != null)
            {
                OnBulletDies.Invoke(this);
            }

        }
    }
}