using UnityEngine;

namespace OvenFresh
{
    public enum InteractiveType
    {
        PASS,
        BLOCK,
        GOAL 
    }
    [CreateAssetMenu(fileName = "New Tile Type", menuName = "Week1/Tile Type", order = 0)]
    public class TileType : ScriptableObject
    {
        public Color color;
        public InteractiveType interactiveType;
    }
}