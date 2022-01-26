using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace OvenFresh.Week1_GlobalGameJam.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Board Configuration", menuName = "Week1/Board Config", order = 0)]
    public class BoardConfiguration : ScriptableObject
    {
        [Tooltip("Width for the board, wall will be the edge")]
        [Range(3,20)]
        public int width;
        
        [Tooltip("Height for the board, wall will be the edge")]
        [Range(3,20)]
        public int height;

        public TileType groundTileType;
        public TileType wallTileType;
        public MoverType moverType;
        public Texture2D texture; //figure out how to map the texture later
        
    }
}