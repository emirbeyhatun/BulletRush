using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class PlayerInput
    {
        private Rigidbody rb;
        private Camera cam;

        public PlayerInput(Rigidbody rb, Camera cam)
        {
            this.rb = rb;
            this.cam = cam;
        }

        public void MovementInputs(float speed, Vector2 joystickDir)
        {
            if (rb && cam)
            {
                Vector3 movementDirection = Quaternion.Euler(0, cam.transform.localRotation.eulerAngles.y, 0) * new Vector3(joystickDir.x, 0, joystickDir.y);
                rb.velocity = movementDirection * speed;
            }
        }
    }
}