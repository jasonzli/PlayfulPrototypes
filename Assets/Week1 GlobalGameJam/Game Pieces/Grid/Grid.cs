using System;
using System.Collections;
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
        
        private int _width;

        public int Width
        {
            get { return _width; }
        }
        private int _height;
        
        public int Height
        {
            get { return _height; }
        }

        private Mover _mover;
        private Vector3Int _moverPosition;
        public Vector3Int MoverPosition
        {
            get { return _moverPosition; }
            private set { _moverPosition = value; }
        }
        
        private Tile[,] _allTiles;
        private Vector3Int _moverStartingPosition;
        private bool _isAnimating;

        public bool IsAnimating
        {
            get { return _isAnimating; }
        }
        private Vector3 _gridCenteringOffset;

        void Awake()
        {
            
        }

        public void Init(BoardConfiguration _config)
        {
            config = _config;
        }

        public Tile[,] SetupBoard(BoardConfiguration _config = null)
        {
            if (_config != null) config = _config;
            
            _width = config.mapTexture.width;
            _height = config.mapTexture.height;
            _gridCenteringOffset = new Vector3((_width - 1) * .5f, (_height - 1) * .5f, 0);
            _allTiles = new Tile[_width, _height];
            
            //This also sets the moverStartingPosition
            _allTiles = CreateBoard(_width,_height);
            MoverPosition = new Vector3Int (_moverStartingPosition.x,_moverStartingPosition.y,0);

            return _allTiles;
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
                    tileObj.transform.localPosition = new Vector3(i, j,0) - _gridCenteringOffset ;
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
                            _moverStartingPosition = new Vector3Int(i, j, 0);
                            
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
   

        public void MoveToPosition(Vector3 targetPosition, float moveInTime = .5f)
        {
            if (_isAnimating) return;

            //set the guard
            _isAnimating = true;
               
            //Use Coroutine for WEBGL
            StartCoroutine(TravelTo(targetPosition, moveInTime));

        }

        public IEnumerator TravelTo(Vector3 targetPosition, float moveInTime = .5f)
        {
            var origin = transform.localPosition;
            var elapsedTime = 0f;
            var t = 0f;
            while (elapsedTime < moveInTime)
            {
                t = Mathf.Clamp(elapsedTime / moveInTime, 0f, 1f);

                if (config.movementCurve)
                {
                    t = config.movementCurve.Evaluate(t);
                }
                
                //move
                transform.localPosition = Vector3.Lerp(origin, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = targetPosition;
            
            _isAnimating = false; //open guard
        }

        public void ScaleAxisAnimation(Vector3 targetScale, float moveInTime = 0.5f)
        {
            if (_isAnimating) return;

            //set the guard
            _isAnimating = true;

            //Use Coroutine for WEBGL
            StartCoroutine(ScaleAxisRoutine(targetScale,moveInTime));
        }

        private IEnumerator ScaleAxisRoutine(Vector3 targetScale, float moveInTime)
        {
            var originalScale = transform.localScale;
            var elapsedTime = 0f;
            var t = 0f;
            while (elapsedTime < moveInTime)
            {
                t = elapsedTime / moveInTime;
                t = Mathf.Clamp(elapsedTime / moveInTime, 0f, 1f);

                if (config.movementCurve)
                {
                    t = config.movementCurve.Evaluate(t);
                }
                //move
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;

            _isAnimating = false;
        }
    }
}