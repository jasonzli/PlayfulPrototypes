using System;
using UnityEngine;

namespace OvenFresh
{
    public class FlyingObject : MonoBehaviour
    {
        protected FloatParameter _worldSpeed;

        public void Init(FloatParameter speedValue)
        {
            _worldSpeed = speedValue;
        }

        private void Update()
        {
            MoveAlongDirection(-Vector3.forward);
            
            if(transform.position.z < -10f) Destroy(gameObject);
        }   

        private void MoveAlongDirection(Vector3 direction)
        {
            transform.Translate(direction * _worldSpeed.value * Time.deltaTime);
        }
    }
}