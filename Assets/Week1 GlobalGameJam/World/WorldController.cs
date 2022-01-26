using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * A controller for world 
 * Houses the controls for the camera and the world grids that we transition between
 */
namespace OvenFresh
{
    public class WorldController : MonoBehaviour
    {
        [SerializeField] private Transform camPositionA;
        [SerializeField] private Transform camPositionB;
        [SerializeField] private CurveParameter movementCurve;        
        [SerializeField] private Camera _cam;

        [SerializeField] private Vector3 lookAtTarget;

        private bool _moving;
        private bool _switched = false;
        void Awake()
        {
            if (_cam != null)
            {
                _cam = Camera.main;
            }

            _cam.transform.position = camPositionA.position;
            _cam.transform.rotation = Quaternion.identity;
            
        }

        public async void OnFire(InputValue value)
        {
            
            if (_moving) return;
            
            if (_switched)
            {
                await MoveToWhileLookingAt(camPositionA.position, lookAtTarget, .4f);
                _switched = !_switched;
            }
            else
            {
                await MoveToWhileLookingAt(camPositionB.position, lookAtTarget, .4f);
                _switched = !_switched;
            }
        }
        async Task MoveToWhileLookingAt(Vector3 targetPosition, Vector3 lookAt, float animationTime)
        {
            if (_moving) return;
            
            _moving = true;
            
            var elapsedTime = 0f;
            Vector3 currentPosition = _cam.transform.position;
            Vector3 targetDirection = targetPosition - currentPosition;
            Vector3 newDirection = Vector3.RotateTowards(_cam.transform.forward, targetDirection, 3.14f, 0f);

            while (elapsedTime < animationTime)
            {
                var t = elapsedTime / animationTime;

                t = movementCurve.Evaluate(t);

                _cam.transform.position = Vector3.Slerp(currentPosition, targetPosition, t);

                targetDirection = lookAt - _cam.transform.position;
                newDirection = Vector3.RotateTowards(_cam.transform.forward, targetDirection, 3.14f, 0f);

                _cam.transform.rotation = Quaternion.LookRotation(newDirection);
                
                await Task.Yield();
                elapsedTime += Time.deltaTime;
            }

            _cam.transform.position = targetPosition;
            targetDirection = targetPosition - _cam.transform.position;
            newDirection = Vector3.RotateTowards(_cam.transform.forward, targetDirection, 3.14f, 0f);

            _cam.transform.rotation = Quaternion.LookRotation(newDirection);

            _moving = false;
        }
    }
}
