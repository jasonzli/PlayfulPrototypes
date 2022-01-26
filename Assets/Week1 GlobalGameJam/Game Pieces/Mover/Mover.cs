using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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
        void Slide(int xTarget, int yTarget, int zTarget){
            
        }
        
        //Set the color as needed
        public void SetColor(Color color){
            _materialPropertyBlock.SetColor("_Color", color);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
        //Transition Color
        public async Task TransitionColor(Color color){
            
        }
    }
}
