using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    public class BeatFollower : MonoBehaviour
    {
        [SerializeField] private float BeatTiming = 600f;
        [SerializeField] private float DistanceModifier = 1f;
        [SerializeField] private float BaseDistance = 1f;
        [SerializeField] private CurveParameter MovementCurve;

        void Start()
        {
            StartCoroutine(MoveToBeat());
        }

        IEnumerator MoveToBeat()
        {
            while (true)
            {
                //take the current position and then move forward.
                var position = transform.position;

                var t = 0f;
                var elapsedTime = 0f;
                var origin = transform.position;
                var targetPosition = transform.position + BaseDistance*Vector3.right * DistanceModifier;
                var animationTime = BeatTiming*.001f;
                while (elapsedTime < animationTime)
                {
                    t = elapsedTime / animationTime;
                    t = MovementCurve.Evaluate(t);
                    transform.position = Vector3.Lerp(origin, targetPosition, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                if (transform.position.x > 18)
                {
                    transform.position = new Vector3(0f, transform.position.y, transform.position.z);
                    //Destroy(this.gameObject);
                }
            }
        }
    }
}
