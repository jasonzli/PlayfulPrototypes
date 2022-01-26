using OvenFresh.Week1_GlobalGameJam.Scriptable_Objects;
using UnityEngine;
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

        void SetupBoard(BoardConfiguration config)
        {
            width = config.width;
            height = config.height;
            _allTiles = new Tile[width, height];
            _allTiles = CreateBoard(width,height);
            _mover = CreateMover(0,0, 0);
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
                    tileObj.transform.position = new Vector3(i, j,0);
                    var tileComponent = tileObj.GetComponent<Tile>();

                    //create a wall
                    if (i == 0 || j == 0 || i == xDimension - 1 || j == yDimension - 1)
                    {
                        tileComponent.Init(config.wallTileType,i,j,0);
                        tileObj.name = $"Wall {i},{j},0";
                    }
                    else //create a ground
                    {
                        tileComponent.Init(config.groundTileType,i,j,0);
                        tileObj.name = $"Tile {i},{j},0";
                    }
                    //add tile to array
                    _allTiles[i, j] = tileComponent;
                }
            }

            return newTiles;
        }

        Mover CreateMover(int xPos, int yPos, int zPos)
        {
            var mover = Instantiate(moverPrefab, Vector3.zero, Quaternion.identity);
            mover.transform.position = new Vector3(xPos, yPos, zPos);
            mover.name = "Mover";
            mover.GetComponent<Mover>().Init(config.moverType,0,0,0);
            return mover.GetComponent<Mover>();
        }
    }
}