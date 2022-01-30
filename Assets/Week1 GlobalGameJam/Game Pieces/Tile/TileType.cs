using UnityEngine;

namespace OvenFresh
{
    
    [CreateAssetMenu(fileName = "New Tile Type", menuName = "Week1/Tile Type", order = 0)]
    public class TileType : ScriptableObject
    {
        public Color activeColor;
        public Color inactiveColor;
        public CurveParameter transitionCurve;
        public bool RenderEnabled;
    }
}