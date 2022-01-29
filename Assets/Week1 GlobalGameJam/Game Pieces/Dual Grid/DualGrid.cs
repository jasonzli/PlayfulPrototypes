using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    /*
     * This dual grid is the home for both of the underlying grids.
     * It is what will track both of the grids and also keep track of where the mover is
     * Between the two
     * The two grids themselves have no other control
     */
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
        void Awake()
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
    }
}
