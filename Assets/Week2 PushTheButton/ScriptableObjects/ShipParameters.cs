using UnityEngine;

namespace OvenFresh.Week2_PushTheButton.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Starship Parameters", menuName = "Week2/Starship Parameters", order = 0)]
    public class ShipParameters : ScriptableObject
    {
        public float turningSpeed = 1f;
        public float maxSpeed = 10f;
        public float maxBoost = 1f;
        
    }
}