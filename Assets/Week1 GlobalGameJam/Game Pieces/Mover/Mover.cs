using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/*
this Mover class is the character class that moves around
It should be able to slide along the board.
*/

namespace OvenFresh
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Mover : GridPiece
    {   
    
        [SerializeField]private Color _color;
        public MoverType type;
        private MeshRenderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        private bool isAnimating;
        
        
        void Awake()
        {    
            _renderer = GetComponent<MeshRenderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_materialPropertyBlock);
        }
        
        public void Init(MoverType _type, int _x, int _y, int _z){
            type = _type;
            UpdateIndex(_x,_y,_z);
            
            _renderer = GetComponent<MeshRenderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            
            SetColor(type.color);
        }
        
       
        //Move from currently location to the target
        async void SlideToInTime(int xTarget, int yTarget, int zTarget){
            
        }
        
        //Set the color as needed
        public void SetColor(Color color){
            _materialPropertyBlock.SetColor("_Color", color);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
        //Transition Color
        public async Task TransitionColor(Color color){
            
        }
        
        //Movement
        public void MoveToPosition(Vector3 targetPosition, float moveInTime = .5f)
        {
            if (isAnimating) return;

            //set the guard
            isAnimating = true;
               
            //Use Coroutine for WEBGL
            StartCoroutine(TravelTo(targetPosition, moveInTime));

        }

        public IEnumerator TravelTo(Vector3 targetPosition, float moveInTime = .5f)
        {
            var origin = transform.position;
            var elapsedTime = 0f;
            var t = 0f;
            while (elapsedTime < moveInTime)
            {
                t = Mathf.Clamp(elapsedTime / moveInTime, 0f, 1f);

                if (type.animationCurve)
                {
                    t = type.animationCurve.Evaluate(t);
                }
                
                //move
                transform.position = Vector3.Lerp(origin, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            
            isAnimating = false; //open guard
        }
    }
}
