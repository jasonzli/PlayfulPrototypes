using OvenFresh.Week1_GlobalGameJam.Scriptable_Objects;
using UnityEngine;

namespace OvenFresh
{
    [CreateAssetMenu(fileName = "New Dual Grid Config", menuName = "Week1/Dual Grid Configuration", order = 0)]
    public class DualGridConfiguration : ScriptableObject
    {
        public BoardConfiguration xyGridConfig;
        public BoardConfiguration zyGridConfig;
        public MovementMode startingMode;
    }
}