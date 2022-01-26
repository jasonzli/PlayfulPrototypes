using UnityEngine;

namespace OvenFresh
{
    //all GridPieces exist on a board
    public abstract class GridPiece : MonoBehaviour
    {
        public int xIndex { get; set; }
        public int yIndex { get; set; }
        public int zIndex { get; set; }

        public Vector3 GridPosition()
        {
            return new Vector3(xIndex, yIndex, zIndex);
        }
        
        public void UpdateIndex(int _x,int _y,int _z)
        {
            xIndex = _x;
            yIndex = _y;
            zIndex = _z;
        }

        public void UpdateIndex(Vector3 v)
        {
            xIndex = (int) v.x;
            yIndex = (int) v.y;
            zIndex = (int) v.z;

        }
    }
}