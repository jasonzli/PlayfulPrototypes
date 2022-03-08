using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OvenFresh
{
    public class FlightControl : MonoBehaviour
    {
        
        public FloatParameter Agility;
        public FloatParameter FlightSpeed;

        [SerializeField] float _startSpeed;
        [SerializeField] private float _startAgility;
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
        }

        void Move()
        {
            Vector3 direction = _gamepad.leftStick.ReadValue() * Agility.value * Time.deltaTime;
            Vector3 newPosition = transform.position + direction;
            Vector3 clampPosition = new Vector3(
                Mathf.Clamp(newPosition.x, -9f, 9f),
                Mathf.Clamp(newPosition.y, -4f, 4f),
                0f);
            transform.position = clampPosition;
        }
    }
}
