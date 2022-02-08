using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OvenFresh
{
    public class StarshipController : MonoBehaviour
    {

        public float _heading = 0f;
        public float _rotationDirection = 0f;
        public Vector2 _direction = new Vector2(0, 1);
        public float _speed = 3f;
        public float rotationSpeed = 5f;


        void OnPivot(InputValue value)
        {
            _rotationDirection = value.Get<float>(); //will be a value between -1 and 1;
        }

        void Update()
        {
            UpdateHeading();
            RotateDirectionByHeading(_heading);
            UpdateShipRotation();
            ApplyMovement();
        }

       
        void ApplyMovement()
        {
            transform.localPosition = ApplyVelocityToPosition(transform.localPosition, _direction, _speed);
        }

        //apply velocity in the y direction
        Vector3 ApplyVelocityToPosition(Vector3 position, Vector2 direction, float speed)
        {
            Vector3 newPos = new Vector3(
                position.x + direction.x * speed * Time.deltaTime,
                0,
                position.z = position.z + direction.y * speed * Time.deltaTime
            );
            return newPos;
        }

        float UpdateHeading()
        {
            _heading += _rotationDirection * rotationSpeed;
            _heading %= 360f;
            return _heading;
        }

        void UpdateShipRotation()
        {
            transform.localRotation = Quaternion.AngleAxis(_heading, Vector3.up);
        }
        
        Vector2 RotateDirectionByHeading(float heading)
        {
            _direction = (Quaternion.AngleAxis(-heading, Vector3.forward) * Vector3.up);
            return _direction;
        }
    }
}