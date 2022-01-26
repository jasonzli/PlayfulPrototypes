using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    [CreateAssetMenu(fileName = "New Float Parameter", menuName = "Scriptable Parameters/Float")]
    public class FloatParameter : ScriptableObject
    {
        public float value;
    }
}
