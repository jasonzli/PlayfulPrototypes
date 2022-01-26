using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    
    [CreateAssetMenu(fileName = "New Int Parameter", menuName = "Scriptable Parameters/Integer")]
    public class IntParameter : ScriptableObject
    {
        public int value;
    }
}
