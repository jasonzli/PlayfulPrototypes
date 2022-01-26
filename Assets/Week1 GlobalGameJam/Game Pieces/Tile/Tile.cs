using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Tile : GridPiece
    {
        public TileType type;

        private MeshRenderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        void Awake()
        {
        }
        
        
        public void Init(TileType _type, int _xIndex, int _yIndex, int _zIndex)
        {
            type = _type;
            UpdateIndex(_xIndex,_yIndex,_zIndex);
            
            _renderer = GetComponent<MeshRenderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            
            _materialPropertyBlock.SetColor("_Color", type.color);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
