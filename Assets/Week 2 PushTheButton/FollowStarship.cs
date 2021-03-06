using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    public class FollowStarship : MonoBehaviour
    {
        [Range(.1f,1f)]public float _followSpeed = .9f;
        [Range(0f, 20f)] public float _camHeight = 10f;
        public Transform shipToFollow;
        public Vector3 velocity;
        private void Update()
        {
            MoveToShip();
        }

        void MoveToShip()
        {
            var newPosition = Vector3.SmoothDamp(transform.position, shipToFollow.position, ref velocity,-1f);
            newPosition.y = _camHeight;
            transform.position = newPosition;
        }
    }
}
