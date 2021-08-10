using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    [System.Serializable]
    public class PlayerStats
    {
        public int hp = 100;
        public int bulletDamage = 100;
        public float speed = 5;
        public float bulletSpeed = 5;
        public float fireRate = 5;
        public bool isDead = false;
    }
    public class Player : MonoBehaviour, IDamagable
    {
        public event Action OnDeath;
        [SerializeField]private PlayerStats stats;
        [SerializeField]private Joystick joystick;
        [SerializeField]private Camera cam;
        [SerializeField]private Transform spine;
        [SerializeField]private Transform aimTransform;
        [SerializeField]private GameObject bulletPrefab;
        [SerializeField]private TargetFinder targetFinder;


        private Rigidbody rb;
        private Animator animator;

        public PlayerInput PlayerInput{ get; set; }
        public PlayerAnimationController PlayerAnimationController { get; set; }
        public ShootingController ShootingController { get; set; }


        //These will be used to lerp the spine to its new target that way it wont snap immediately
        private float spineRotationTime = 0.3f;
        private float spineRotationTimer = 0;
        private Enemy spinePreviousTarget;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

            PlayerInput = new PlayerInput(rb, cam);
            PlayerAnimationController = new PlayerAnimationController(animator);

            if (stats != null)
            {
                ShootingController = new ShootingController(stats.fireRate, bulletPrefab, aimTransform, stats.bulletSpeed, stats.bulletDamage, OnEnemyDies);
            }
        }

        private void Update()
        {
            if (targetFinder && targetFinder.GetTarget())
            {
                if (ShootingController != null)
                {
                    ShootingController.Update(Time.deltaTime, targetFinder.GetTarget().transform);
                }
            }
        }
        private void FixedUpdate()
        {
            if (PlayerInput != null && stats != null && joystick)
            {
                PlayerInput.MovementInputs(stats.speed, joystick.Direction);
            }

            LookTowardsTarget();

            if (PlayerAnimationController != null && rb && joystick)
            {
                PlayerAnimationController.UpdateAnimation(transform.forward, rb.velocity.normalized, joystick.Direction.magnitude);
            }
        }

        private void LateUpdate()
        {
            //We need to rotate spine after the animations that's why we do it in the late update
            for (int i = 0; i < 2; i++)
            {
                RotateSpineToAimEnemy();
            }

        }

        private void LookTowardsTarget()
        {
            if (targetFinder)
            {
                Enemy enemy = targetFinder.GetTarget();

                if (enemy)
                {
                    Vector3 dir = (enemy.transform.position - transform.position);
                    dir.y = 0;

                    if(dir != Vector3.zero)
                    {
                        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime);
                    }
                }
            }
        }

        public void OnEnemyDies(Enemy enemy)
        {
            if (targetFinder && enemy)
            {
                targetFinder.RemoveEnemyIfDead(enemy);
            }
        }


        private void RotateSpineToAimEnemy()
        {
            if (targetFinder == null) return;

            Enemy enemy = targetFinder.GetTarget();

            if (enemy)
            {
                if(spinePreviousTarget != enemy)
                {
                    spineRotationTimer = 0;
                    spinePreviousTarget = enemy;
                }
                AimAtTarget(spine, enemy.transform.position);
            }
        }

        void AimAtTarget(Transform bone, Vector3 targetPosition)
        {
            if (aimTransform == null || bone == null) return;

            spineRotationTimer += Time.deltaTime;
            spineRotationTimer = Mathf.Min(spineRotationTimer, spineRotationTime);
            //Rotate spine to target objects direction
            Vector3 aimDirection = aimTransform.forward;
            Vector3 targetDir = targetPosition - aimTransform.position;
            Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDir);
            Quaternion newRotation = aimTowards * bone.rotation;

            bone.rotation = Quaternion.Lerp(bone.rotation, newRotation, spineRotationTimer / spineRotationTime);

        }

        public void SetStats(PlayerStats stats)
        {
            this.stats = stats;
        }

        public void SetTargetFinder(TargetFinder newFinder)
        {
            targetFinder = newFinder;
        }

        public bool TakeDamage(int damage, out bool isDead)
        {
            isDead = false;

            if (stats == null)        return false;
            if (stats.isDead == true) return false;

            stats.hp -= damage;
            stats.hp = Mathf.Max(0, stats.hp);
            if (stats.hp == 0)
            {
                stats.isDead = true;
                isDead = true;
                OnDeath?.Invoke();

            }

            return true;
        }

        public bool IsDead()
        {
            if (stats == null) return true;

            return stats.isDead;
        }
    }
}
