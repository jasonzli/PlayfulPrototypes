using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OvenFresh
{
    /*
     * This dual grid is the home for both of the underlying grids.
     * It is what will track both of the grids and also keep track of where the mover is
     * Between the two
     * The two grids themselves have no other control
     */

    public enum MovementMode
    {
        XY,
        ZY
    }
    
    public class DualGrid : MonoBehaviour
    {

        public DualGridConfiguration dualConfig;
        [SerializeField] private GameObject emptyGridPrefab;
        public Grid gridXY;
        public Grid gridZY;
        
        [SerializeField] private GameObject moverPrefab;

        private Tile[,] _allXYTiles;
        private Tile[,] _allZYTiles;
        private Mover _mover;
        [SerializeField] private MovementMode _mode;
        
        private void Awake()
        {
            CreatePuzzleObject();
        }

        //We create the grid centered on the coordinate of the DualGrid object
        public void CreatePuzzleObject()
        {
            //Create the two grids, get the mover positions from both
            gridXY = Instantiate(emptyGridPrefab, Vector3.zero, Quaternion.identity,transform).GetComponent<Grid>();
            gridZY = Instantiate(emptyGridPrefab, Vector3.zero,Quaternion.AngleAxis(-90f,Vector3.up),transform ).GetComponent<Grid>();

            gridXY.transform.name = "GridXY";
            gridZY.transform.name = "GridZY";
            
            //Give initial grid data
            gridXY.Init(dualConfig.xyGridConfig);
            gridZY.Init(dualConfig.zyGridConfig);
            
            //call to setup board and get all tile data localized
            _allXYTiles = gridXY.SetupBoard();
            _allZYTiles = gridZY.SetupBoard();
            
            //assemble the new mover position
            var moverXY = gridXY.MoverPosition; //x position here is the x position of the ZY grid
            var moverZY = gridZY.MoverPosition; //x position here is the z position of the XY grid
            
            //grid positions
            gridXY.transform.localPosition = new Vector3(0, 0, moverZY.x);
            gridZY.transform.localPosition = new Vector3(moverXY.x,0,0);
            
            //This is the object's position
            var combinedMoverPosition = new Vector3(moverXY.x, moverZY.y, moverZY.x);
            
            //place the grids and mover into position
            _mover = CreateMover(dualConfig.xyGridConfig.moverType,combinedMoverPosition);
            //track all tiles

        }
        
        
        
        //Creating the Mover
        Mover CreateMover(MoverType type, Vector3 target)
        {
            Vector3Int position = Vector3Int.RoundToInt(target);
            return CreateMover(type, position.x, position.y, position.z);
        }
        Mover CreateMover(MoverType type, int xPos, int yPos, int zPos)
        {
            var mover = Instantiate(moverPrefab, Vector3.zero, Quaternion.identity, transform);
            mover.transform.localPosition = transform.TransformPoint(new Vector3(xPos , yPos, zPos));
            mover.name = "Mover";
            mover.GetComponent<Mover>().Init(type,xPos,yPos,zPos);
            return mover.GetComponent<Mover>();
        }

        void OnFire()
        {
            if (_mode != MovementMode.XY)
            {
                _mode = MovementMode.XY;
            }
            else
            {
                _mode = MovementMode.ZY;
            }
        }
        
        void OnMove(InputValue value)
        {
            if (_mover == null) return;
            
            var dir = value.Get<Vector2>();
            
            //only trigger if direction valid
            if (dir.magnitude < 1f) return; //rightn ow just to catch the 0,0 that appears
            
            //Check the mode
            Tile[,] gridToCheck = new Tile[1,1];
            Vector2 gridIndex = new Vector2();
            TileType wallType = dualConfig.xyGridConfig.wallTileType;
            TileType goalType = dualConfig.xyGridConfig.goalTileType;
            //set the values for checking
            //set the grid that we check
            //set the index in that grid based on the mover's index;
            if (_mode == MovementMode.XY)
            {
                gridToCheck = _allXYTiles;
                wallType = dualConfig.xyGridConfig.wallTileType;
                gridIndex = new Vector2(_mover.xIndex, _mover.yIndex);
            }else if (_mode == MovementMode.ZY)
            {
                gridToCheck = _allZYTiles;
                wallType = dualConfig.zyGridConfig.wallTileType;
                gridIndex = new Vector2(_mover.xIndex, _mover.yIndex);
            }
            
            //scan the grid in a direction until we hit a wall 
            do
            {
                if (gridIndex.x < 0 || gridIndex.x >= gridToCheck.GetLength(0) || gridIndex.y < 0 || gridIndex.y >= gridToCheck.GetLength(1)) break;
               
                if (gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == wallType||
                    gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == goalType) break;
                gridIndex += dir;

            } while (gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type != wallType||
                     gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == goalType);
            
            
            //we've found the index,
            gridIndex -= dir;//decrement back

            //update position based on movement mode
            //do this by transforming the local point
            //also set the grid to be parented
            Vector3 target = new Vector3();
            Grid gridToMove = gridXY;
            if (_mode == MovementMode.XY)
            { 
                target = transform.TransformPoint(new Vector3(gridIndex.x, gridIndex.y, _mover.transform.position.z));
                gridToMove = gridZY;
                gridToMove.MoveToPosition(new Vector3(target.x, 0, 0));
            }else if (_mode == MovementMode.ZY)
            {
                target = transform.TransformPoint(new Vector3(_mover.transform.position.x, gridIndex.y, gridIndex.x));
                gridToMove = gridXY;
                gridToMove.MoveToPosition(new Vector3(0, 0, target.z));
            }
            
            
            //send the move command
            _mover.MoveToPosition(target,.5f);
            _mover.UpdateIndex((int) gridIndex.x, (int) gridIndex.y,0);

        }
    }
}
