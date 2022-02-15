using UnityEngine;

namespace OvenFresh
{
    [CreateAssetMenu(fileName = "New Starship States", menuName = "Week2/Starship States", order = 0)]
    public class ShipStates : ScriptableObject
    {
        public ShipStateParameters DriveState;
        public ShipStateParameters ChargeState;
    }
}