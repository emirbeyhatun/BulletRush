using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class PlayerAnimationController
    {
        public string ForwardAnim = "WalkForward";
        public string BackwardsAnim = "WalkBackwards";
        public string RightwayAnim = "WalkRightway";
        public string LeftwayAnim = "WalkLeftway";

        private Animator animator;

        public PlayerAnimationController(Animator animator)
        {
            this.animator = animator;
        }

        public void UpdateAnimation(Vector3 forwardDirection, Vector3 velocityDirection, float joystickMagnitude)
        {
            if (animator == null) return;


            float angle = Vector3.SignedAngle(forwardDirection, velocityDirection, Vector3.up);

            animator.SetFloat(ForwardAnim, 0);
            animator.SetFloat(RightwayAnim, 0);
            animator.SetFloat(BackwardsAnim, 0);
            animator.SetFloat(LeftwayAnim, 0);

            if (joystickMagnitude <= 0.1f) return;

            if (Mathf.Abs(angle) <= 45)
            {
                animator.SetFloat(ForwardAnim, 1);
            }
            else if (angle > 45 && angle <= 135)
            {
                animator.SetFloat(RightwayAnim, 1);
            }
            else if (angle > 135 && angle <= 180 || angle >= -180 && angle < -135)
            {
                animator.SetFloat(BackwardsAnim, 1);
            }
            else if (angle < -45 && angle >= -135)
            {
                animator.SetFloat(LeftwayAnim, 1);
            }
        }

    }
}