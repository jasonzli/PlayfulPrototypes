using UnityEngine;

namespace OvenFresh
{
    [CreateAssetMenu(fileName = "New Starship State", menuName = "Week2/Starship State", order = 0)]
    public class ShipStateParameters : ScriptableObject
    {
        public float Acceleration = 1.0f;
        public float MaxVelocity = 5f;
        [Range(0, 1f)] public float DragReduction = 1f;
        public float MaxPivotSpeed = .5f;
        public float PivotAcceleration = .7f;
    }
}