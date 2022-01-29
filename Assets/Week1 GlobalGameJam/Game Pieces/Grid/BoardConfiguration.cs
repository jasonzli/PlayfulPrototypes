using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace OvenFresh.Week1_GlobalGameJam.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Board Configuration", menuName = "Week1/Board Config", order = 0)]
    public class BoardConfiguration : ScriptableObject
    {
        public TileType groundTileType;
        public TileType wallTileType;
        public TileType goalTileType;
        public MoverType moverType; //each board has a different mover type that you transition between
        [Tooltip("Black for walls, white for ground, red for goal, blue for mover. Note that mover is also the position of the perpendicular grid")]
        public Texture2D mapTexture;
        
    }
}