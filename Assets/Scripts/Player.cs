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
        public float speed = 5;
        public bool isDead = false;
    }
    public class Player : MonoBehaviour, IDamagable
    {
        public event Action OnDeath;
        [SerializeField]private PlayerStats stats;
        [SerializeField]private Joystick joystick;
        [SerializeField]private Camera cam;
        [SerializeField]private TargetFinder targetFinder;
        [SerializeField]private Transform spine;
        [SerializeField]private Transform aimTransform;
        private Rigidbody rb;
        private Animator animator;

        public PlayerInput PlayerInput{ get; set; }

        public PlayerAnimationController PlayerAnimationController { get; set; }


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

            PlayerInput = new PlayerInput(rb, cam);
            PlayerAnimationController = new PlayerAnimationController(animator);
        }
        
        private void Update()
        {
            if (PlayerInput != null && stats != null && joystick)
            {
                PlayerInput.MovementInputs(stats.speed, joystick.Direction);
            }

            LookTowardsTarget();

            if (PlayerAnimationController != null && rb && joystick)
            {
                PlayerAnimationController.UpdateAnimaton(transform.forward, rb.velocity.normalized, joystick.Direction.magnitude);
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < 2; i++)
            {
                RotateSpineToAimEnemy();
            }

        }

        private void LookTowardsTarget()
        {
            if (targetFinder)
            {
                EnemyBase enemy = targetFinder.GetTarget();

                if (enemy)
                {
                    Vector3 dir = (enemy.transform.position - transform.position);
                    dir.y = 0;

                    if(dir != Vector3.zero)
                    {
                        transform.forward = dir.normalized;
                    }
                }
            }
        }

        private void RotateSpineToAimEnemy()
        {
            EnemyBase enemy = targetFinder.GetTarget();

            if (enemy)
            {
                AimAtTarget(spine, enemy.transform.position);
            }
        }


        void AimAtTarget(Transform bone, Vector3 targetPosition)
        {
            if (aimTransform == null || bone == null) return;
            //Rotate spine to target objects direction
            Vector3 aimDirection = aimTransform.forward;
            Vector3 targetDir = targetPosition - aimTransform.position;
            Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDir);
            bone.rotation = aimTowards * bone.rotation;

        }

        public void SetStats(PlayerStats stats)
        {
            this.stats = stats;
        }

        //private void MovementInputs()
        //{
        //    if (rb && joystick && cam)
        //    {
        //        Vector3 movementDirection = Quaternion.Euler(0, cam.transform.localRotation.eulerAngles.y, 0) * (new Vector3(joystick.Direction.x, 0, joystick.Direction.y));
        //        rb.velocity = movementDirection * stats.speed;
        //    }
        //}

        public bool TakeDamage(int damage)
        {
            if (stats == null)        return false;
            if (stats.isDead == true) return false;

            stats.hp -= damage;
            stats.hp = Mathf.Max(0, stats.hp);
            if (stats.hp == 0)
            {
                stats.isDead = true;
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
