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

        
        public void Init(TileType _type, int _xIndex, int _yIndex, int _zIndex)
        {
            type = _type;
            UpdateIndex(_xIndex,_yIndex,_zIndex);
            
            _renderer = GetComponent<MeshRenderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_materialPropertyBlock);
            
            _materialPropertyBlock.SetColor("_Color", type.activeColor);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
            
            EnableRenderer(type.RenderEnabled);
        }

        public void EnableRenderer(bool on)
        {
            if (on) _renderer.enabled = true;
            else _renderer.enabled = false;
        }

        public void FadeAnimation(bool on, float animationTime)
        {
            StartCoroutine(FadeRoutine(on, animationTime));
        }

        private IEnumerator FadeRoutine(bool on, float animationTime)
        {
            var t = 0f;
            var elapsedTime = 0f;
            Color targetColor = type.inactiveColor;
            if (on)
            {
                targetColor = type.activeColor;
            }

            Color currentColor = _materialPropertyBlock.GetColor("_Color");
            while (elapsedTime < animationTime)
            {
                t = Mathf.Clamp(elapsedTime / animationTime, 0, 1);

                t = type.transitionCurve.Evaluate(t);
                

                var c = Color.Lerp(currentColor, targetColor, t);

                //set the material properties
                _materialPropertyBlock.SetColor("_Color", c);
                _renderer.SetPropertyBlock(_materialPropertyBlock);
                
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _materialPropertyBlock.SetColor("_Color", targetColor);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
            

        }

    }
}
