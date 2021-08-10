using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 distance;

        Vector3 lastFramePosition;
        private Vector3 velocity = Vector3.zero;

        private void LateUpdate()
        {
            lastFramePosition = transform.position;
            if (target)
            {
                transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + distance, ref velocity, 0.5f);
            }
        }
    }
}