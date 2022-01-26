using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    
    [CreateAssetMenu(fileName = "New Boolean Parameter", menuName = "Scriptable Parameters/Bool")]
    public class BoolParameter : ScriptableObject
    {
        public bool value;
    }
}
