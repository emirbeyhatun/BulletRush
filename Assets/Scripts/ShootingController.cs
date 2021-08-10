using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class ShootingController
    {
        private GameObject bulletPrefab;
        private Transform bulletSpawnPos;
        private float fireRate = 1;//per second
        private float bulletSpeed;
        private float timer = 0;
        private int damage = 0;
        private List<Bullet> bulletPool = new List<Bullet>(20);
        private Action<Enemy> OnEnemyDies;

        public ShootingController(float fireRate, GameObject bulletPrefab, Transform bulletSpawnPos, float bulletSpeed, int damage, Action<Enemy> OnEnemyDies)
        {
            this.bulletPrefab = bulletPrefab;
            this.fireRate = fireRate;
            this.bulletSpawnPos = bulletSpawnPos;
            this.bulletSpeed = bulletSpeed;
            this.damage = damage;
            this.OnEnemyDies = OnEnemyDies;
        }

        public void Update(float deltaSeconds, Transform target)
        {
            if (fireRate <= 0 || target == null) return;

            timer += deltaSeconds;
            if(timer > 1 / fireRate)
            {
                timer = 0;
                Shoot(target);
            }
        }

        public void Shoot(Transform target)
        {
            Bullet bullet = null;

            if(bulletPool != null && bulletPool.Count > 0)
            {
                bullet = bulletPool[0];
                bulletPool.RemoveAt(0);
            }
            else
            {
                bullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity).GetComponent<Bullet>();
            }

            if (bullet)
            {
                bullet.OnBulletDies = PoolBullet;
                bullet.OnEnemyDies = OnEnemyDies;
                bullet.gameObject.SetActive(false);
                bullet.transform.position = bulletSpawnPos.position;
                bullet.Launch((target.transform.position - bulletSpawnPos.position).normalized, damage, bulletSpeed);
            }
        }

        public void PoolBullet(Bullet bullet)
        {
            if(bulletPool != null && bulletPool.Contains(bullet) == false)
            {
                bulletPool.Add(bullet);
            }
        }
    }
}