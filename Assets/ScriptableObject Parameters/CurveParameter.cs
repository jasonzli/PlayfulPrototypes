using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    [CreateAssetMenu(fileName = "New Curve Parameter", menuName = "Scriptable Parameters/Curve")]
    public class CurveParameter : ScriptableObject
    {
        public AnimationCurve curve;

        public float Evaluate(float t)
        {
            return curve.Evaluate(t);
        }
    }
}
