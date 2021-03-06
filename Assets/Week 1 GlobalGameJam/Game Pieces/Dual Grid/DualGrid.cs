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
    
    [RequireComponent(typeof(DualLevelController))]
    public class DualGrid : MonoBehaviour
    {

        public DualGridConfiguration dualConfig;
        [SerializeField] private GameObject emptyGridPrefab;
        public Grid gridXY;
        public Grid gridZY;
        
        [SerializeField] private GameObject moverPrefab;

        private Tile[,] _allXYTiles;
        private Tile[,] _allZYTiles;
        private Vector3 _gridOffset;
        private Mover _mover;
        [SerializeField] private MovementMode _mode;
        private bool _isAnimating;

        public bool IsAnimating
        {
            get { return _isAnimating; }
        }
        
        public void ClearData()
        {
            transform.DeleteChildren();
        }

        public void UpdateDualGridData(DualGridConfiguration _config)
        {
            dualConfig = _config;
        }
        public void Reset()
        {
            ClearData();
            CreatePuzzleObject();
        }
        
        

        //We create the grid centered on the coordinate of the DualGrid object
        public void CreatePuzzleObject()
        {
            //reset orientation
            transform.localRotation = Quaternion.identity;
            //start in XY
            _mode = dualConfig.startingMode;

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
            
            _gridOffset = new Vector3( (gridXY.Width-1)*.5f, (gridXY.Height-1)*.5f, (gridZY.Width-1)*.5f);
            
            //grid positions //alignment with centered grid value was discovered by trial and error
            gridXY.transform.localPosition = new Vector3(0, 0, -_gridOffset.z+moverZY.x );
            gridZY.transform.localPosition = new Vector3(-_gridOffset.x+moverXY.x,0,0);
            
            //This is the object's position
            var combinedMoverPosition = new Vector3(moverXY.x, moverZY.y, moverZY.x);
            
            //place the grids and mover into position
            _mover = CreateMover(dualConfig.xyGridConfig.moverType,combinedMoverPosition);

            //fade out ZY grid;
            gridZY.FadeOutWalls(.15f);

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
            mover.transform.localPosition = transform.TransformPoint(new Vector3(xPos , yPos, zPos))-_gridOffset;
            mover.name = "Mover";
            mover.GetComponent<Mover>().Init(type,xPos,yPos,zPos);
            return mover.GetComponent<Mover>();
        }

        
        //This is the on mouse and spacebar behavior
        //This needs to be taken out of the OnFire and be indirected to a collection of functions
        //Ideally this would delegate between the two.
        void OnFire()
        {
            if (IsAnimating || gridXY.IsAnimating || gridZY.IsAnimating || _mover.IsAnimating) return;
            
            //check if the corresponding position is trapped in a wall
            if(IndexIsInGridWall(0,0,gridXY)){}
            
            if (_mode != MovementMode.XY)
            {
                _mode = MovementMode.XY;
                gridZY.FadeOutWalls(.5f);
                gridXY.FadeInWalls(.5f);
                StartCoroutine(RotateSelf(Quaternion.AngleAxis(0, Vector3.up), .5f));
            }
            else
            {
                _mode = MovementMode.ZY;
                gridXY.FadeOutWalls(.5f);
                gridZY.FadeInWalls(.5f);
                
                StartCoroutine(RotateSelf(Quaternion.AngleAxis(90, Vector3.up), .5f));
            }
        }

        public bool IndexIsInGridWall(int xIndex, int yIndex, Grid grid)
        {
            return false;
        }
        private IEnumerator RotateSelf(Quaternion targetRotation, float moveInTime = .5f)
        {
            _isAnimating = true;
            var t = 0f;
            var elapsedTime = 0f;
            var oldOrientation = transform.localRotation;
            while (elapsedTime < moveInTime)
            {
                t = Mathf.Clamp(elapsedTime / moveInTime, 0f, 1f);

                if (dualConfig.movementCurve)
                {
                    t = dualConfig.movementCurve.Evaluate(t);
                }
                
                //move
                transform.localRotation = Quaternion.Lerp(oldOrientation,targetRotation,t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = targetRotation;
            
            _isAnimating = false; //open guard
        }
        
        public void ScaleOut(Vector3 targetScale, float animationTime = .3f)
        {
            _isAnimating = true;
            StartCoroutine(ScaleAxisRoutine(targetScale,animationTime));
        }

        IEnumerator ScaleAxisRoutine(Vector3 targetScale, float animationTime = .5f)
        {
            var t = 0f;
            var elapsedTime = 0f;
            var oldScale = transform.localScale;
            while (elapsedTime < animationTime)
            {
                t = Mathf.Clamp(elapsedTime / animationTime, 0, 1);
                t = dualConfig.movementCurve.Evaluate(t);

                transform.localScale = Vector3.Lerp(oldScale, targetScale, t);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
            _isAnimating = false;
        }
        
        public void MoveTowards(Vector3 targetPosition, float animationTime = .3f)
        {
            _isAnimating = true;
            StartCoroutine(MoveTowardsRoutine(targetPosition,animationTime));
        }

        IEnumerator MoveTowardsRoutine(Vector3 targetPosition, float animationTime)
        {
            var t = 0f;
            var elapsedTime = 0f;
            var oldPosition = transform.localPosition;
            while (elapsedTime < animationTime)
            {
                t = Mathf.Clamp(elapsedTime / animationTime, 0, 1);
                t = dualConfig.movementCurve.Evaluate(t);

                transform.localPosition = Vector3.Lerp(oldPosition, targetPosition, t);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = targetPosition;
            _isAnimating = false;
        }

        public void SpinInPlace(float animationTime = .75f)
        {
            StartCoroutine(SpinInPlaceRoutine(animationTime));
        }

        IEnumerator SpinInPlaceRoutine(float animationTime = .75f)
        {
            //spin on two axes
            var t = 0f;
            var elapsedTime = 0f;
            //create a spin in two axes.
            while (elapsedTime < animationTime)
            {
                t = Mathf.Clamp(elapsedTime / animationTime, 0, 1);
                t = dualConfig.movementCurve.Evaluate(t);

                transform.localRotation = 
                    Quaternion.AngleAxis(1f*360f * t , Vector3.up) *
                    Quaternion.AngleAxis(1f*360f * t, Vector3.right);
                    
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = Quaternion.identity;
            _isAnimating = false;
        }
        
        //This movement action is begging for refactor
        /// <summary>
        /// Ok so this function is a bit fucked
        /// It takes the moving direction from the input value, which is fortunately passed as a Vec2
        /// But hen we need to transform that into a scan
        /// So we can the appropriately aligned grid in the direction of the movement for the wall or goal
        /// if it's a wall, we stop and decrement so we go the place before it
        /// If it's a goal, we go to it
        ///
        /// Once we figureo ut where we're going, we initiate the movement of both the grid and the mover
        /// Each of these has to be handled in a move specific way, which is problematic.
        /// It would be easier if we defined the each state as a searchable and a follower grid.
        /// Consider that next time, because this current method requires a lot of duplication of indices 
        /// </summary>
        public static Action GoalReached;
        void OnMove(InputValue value)
        {
            
            if (_mover == null) return; //check if we even have a mover
            if (IsAnimating || gridXY.IsAnimating || gridZY.IsAnimating || _mover.IsAnimating) return; //animation guard

            var dir = value.Get<Vector2>(); //direction of input
            
            //only trigger if direction valid
            if (dir.magnitude < 1f) return; //rightn ow just to catch the 0,0 that appears
            
            //Check the correct grid based on the mode
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
                gridIndex = new Vector2(_mover.zIndex, _mover.yIndex);
            }
            
            //check immediately if it's a wallTile and do nothing
            if (gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == wallType) return ;
            
            //scan the grid in a direction until we hit a wall 
            bool goalFound = false;
            do
            {
                if (gridIndex.x < 0 || gridIndex.x >= gridToCheck.GetLength(0) || gridIndex.y < 0 || gridIndex.y >= gridToCheck.GetLength(1)) break;
               
                if (gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == wallType) break;
                if (gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == goalType)
                {
                    goalFound = true;
                    break;
                }
                
                gridIndex += dir;

            } while (gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type != wallType||
                     gridToCheck[(int) gridIndex.x, (int) gridIndex.y].type == goalType);
            
            //found the target Index, let's move the thing, but let's check the goal first!
            if (goalFound)
            {
                gridIndex = gridIndex;
                print("Goal Found!");
            }
            else
            {
                gridIndex -= dir;//decrement back so we don't go into the wall
            }
            
            //update position based on movement mode
            //do this by transforming the local point
            //also set the grid to be parented
            Vector3 target = new Vector3();
            Grid gridToMove = gridXY;
            
            if (_mode == MovementMode.XY)
            { 
                target = new Vector3(gridIndex.x - _gridOffset.x, gridIndex.y -_gridOffset.y, gridXY.transform.localPosition.z);
                gridToMove = gridZY;
                gridToMove.MoveToPosition(new Vector3(target.x, 0, 0)); //follow the mover
                //send the move command
                _mover.MoveToPosition(target,.5f);
                _mover.UpdateIndex((int) gridIndex.x, (int) gridIndex.y,_mover.zIndex); //correctly update index
            }else if (_mode == MovementMode.ZY)
            {
                target = new Vector3(gridZY.transform.localPosition.x, gridIndex.y - _gridOffset.y, gridIndex.x - _gridOffset.z);
                gridToMove = gridXY;
                gridToMove.MoveToPosition((new Vector3(0, 0, target.z))); //follow the mover
                
                //send the move command
                _mover.MoveToPosition(target,.5f);
                _mover.UpdateIndex(_mover.xIndex, (int) gridIndex.y,(int) gridIndex.x);
            }

            if (goalFound)
            {
                if (GoalReached != null)
                {
                    StartCoroutine(RunFunctionAfterDelay(GoalReached, .4f));
                }
            }

        }

        //a helper that just lets us do the equivalent of an invoke
        private IEnumerator RunFunctionAfterDelay(Action action, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            
            action();
        }
    }
}
