using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OvenFresh
{
    public class FlightControl : MonoBehaviour
    {
        
        public FloatParameter Agility;
        public FloatParameter FlightSpeed;
        public FloatParameter SpinTime;

        public FloatParameter BoostTime;
        public FloatParameter BoostAmount;
        public float DecayAmount;


        [SerializeField] float _startSpeed;
        [SerializeField] private float _startAgility;
        [SerializeField] private Vector2 _XYBounds;
        private Gamepad _gamepad;
        

        void Awake()
        {
            Reset();
        }

        void Reset()
        {
            _gamepad = Gamepad.current;
            FlightSpeed.value = _startSpeed;
            Agility.value = _startAgility;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            Decay();
        }

        void Decay()
        {
            FlightSpeed.value = Mathf.Clamp(FlightSpeed.value * DecayAmount, _startSpeed, Mathf.Infinity);
        }
        void Move()
        {
            Vector3 direction = _gamepad.leftStick.ReadValue() * Agility.value * Time.deltaTime;
            Vector3 newPosition = transform.position + direction;
            Vector3 clampPosition = new Vector3(
                Mathf.Clamp(newPosition.x, -_XYBounds.x, _XYBounds.x),
                Mathf.Clamp(newPosition.y, -_XYBounds.y, _XYBounds.y),
                0f);
            transform.position = clampPosition;
        }

        public static Action RingHit;
        private void OnCollisionEnter(Collision collision)
        {
            //confirm if the collision is a ring
            if (collision.gameObject.name == "Icosphere")
            {
                StartCoroutine(Spin(SpinTime.value));
                StartCoroutine(Boost(BoostTime.value));
                if (RingHit != null)
                {
                    RingHit();
                }
            }
            
        }

        IEnumerator Boost(float amountOfBoost)
        {
            var elapsedTime = 0f;
            while (elapsedTime < amountOfBoost)
            {
                FlightSpeed.value *= BoostAmount.value;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }
        
        IEnumerator Spin(float spinTime)
        {
            var elapsedTime = 0f;
            while (elapsedTime < spinTime)
            {
                var t = elapsedTime / spinTime;
                
                transform.localRotation = Quaternion.AngleAxis(t * 360f, Vector3.forward);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = Quaternion.identity;
        }

    }
}
