using System;
using OvenFresh.Week1_GlobalGameJam.Scriptable_Objects;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace OvenFresh
{
    public class Grid : MonoBehaviour
    {
        public BoardConfiguration config;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject moverPrefab;
        
        private int width;
        private int height;

        private Mover _mover;
        private Tile[,] _allTiles;

        void Awake()
        {
            SetupBoard(config);
        }

        public void SetupBoard(BoardConfiguration config)
        {
            width = config.mapTexture.width;
            height = config.mapTexture.height;
            _allTiles = new Tile[width, height];
            _allTiles = CreateBoard(width,height);
            //_mover = CreateMover(2,2, 0);
        }
        
        Tile[,] CreateBoard(int xDimension, int yDimension)
        {
            var newTiles = new Tile[xDimension,yDimension];
            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    //only create a new tile if it's empty
                    if (_allTiles[i, j] != null)
                    {
                        //don't recreate them
                        newTiles[i, j] = _allTiles[i, j];
                        continue;
                    }
                    
                    //create tile
                    var tileObj = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, transform);
                    tileObj.transform.localPosition = new Vector3(i - width*.5f, j - height*.5f,0) ;
                    tileObj.transform.localRotation = Quaternion.identity;
                    var tileComponent = tileObj.GetComponent<Tile>();
                    
                    //sample the texture
                    if (config.mapTexture == null)
                    {
                        //create a wall
                        if (i == 0 || j == 0 || i == xDimension - 1 || j == yDimension - 1 ||
                            UnityEngine.Random.Range(0f, 1f) < .3f)
                        {
                            tileComponent.Init(config.wallTileType, i, j, 0);
                            tileObj.name = $"Wall {i},{j},0";
                        }
                        else //create a ground
                        {
                            tileComponent.Init(config.groundTileType, i, j, 0);
                            tileObj.name = $"Tile {i},{j},0";
                        }
                    }
                    else
                    {
                        Color mapColor = config.mapTexture.GetPixel(i, j);
                        
                        if (mapColor == Color.black) //wall
                        {
                            tileComponent.Init(config.wallTileType, i, j, 0);
                            tileObj.name = $"Wall {i},{j},0";
                        }
                        if (mapColor == Color.white) //ground
                        {
                            tileComponent.Init(config.groundTileType, i, j, 0);
                            tileObj.name = $"Ground {i},{j},0";
                        }
                        if (mapColor == Color.blue) //mover + ground
                        {
                            tileComponent.Init(config.groundTileType, i, j, 0);
                            tileObj.name = $"Ground {i},{j},0";
                            //also create mover
                            _mover = CreateMover(i, j, 0);
                        }
                        if (mapColor == Color.red) //goal
                        {
                            tileComponent.Init(config.goalTileType, i, j, 0);
                            tileObj.name = $"Goal {i},{j},0";
                        }
                        
                    }
                    //add tile to array
                    newTiles[i, j] = tileComponent;
                }
            }

            return newTiles;
        }
        
        //Read the texture from the board config to make a board
        Tile[,] CreateBoardFromTexture(int _width, int _height)
        {
            return new Tile[_width, _height];
        }
        Mover CreateMover(int xPos, int yPos, int zPos)
        {
            var mover = Instantiate(moverPrefab, Vector3.zero, Quaternion.identity);
            mover.transform.position = transform.TransformPoint(new Vector3(xPos - width*.5f, yPos - height*.5f, zPos));
            mover.name = "Mover";
            mover.GetComponent<Mover>().Init(config.moverType,xPos,yPos,zPos);
            return mover.GetComponent<Mover>();
        }

        void OnMove(InputValue value)
        {
            if (_mover == null) return;
            var dir = value.Get<Vector2>();
            //only trigger if direction valid
            if (dir.magnitude < 1f) return; //rightn ow just to catch the 0,0 that appears
            
            //get the direction and increment until the wall is unpassable
            var testIndex = new Vector2(_mover.xIndex, _mover.yIndex);
            
            
            //scan the grid in a direction until we hit a wall 
            do
            {
                if (testIndex.x < 0 || testIndex.x >= width || testIndex.y < 0 || testIndex.y >= height) break;
               
                if (_allTiles[(int) testIndex.x, (int) testIndex.y].type == config.wallTileType) break;
                testIndex += dir;

            } while (_allTiles[(int) testIndex.x, (int) testIndex.y].type != config.wallTileType);
            
            
            //we've found the index,
            testIndex -= dir;//decrement back


            //compute the vector3 world coord that mover must go to
            var target = transform.position + new Vector3(testIndex.x - width*.5f, testIndex.y - height*.5f, 0);
            target = transform.localToWorldMatrix * target;

            target = transform.TransformPoint(new Vector3(testIndex.x-width*.5f, testIndex.y -height*.5f, 0));
            
            //send the move command
            _mover.MoveToPosition(target,.5f);
            _mover.UpdateIndex((int) testIndex.x, (int) testIndex.y,0);

        }

        Vector2Int FarthestOpenTileInDirection(Vector2 dir)
        {
            return new Vector2Int();
        }
    }
}