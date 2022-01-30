using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace OvenFresh
{
    public static class Helpers
    {
        //Single call to Camera.main so you can get it by reference later
        private static Camera _camera;
        public static Camera Camera{
            get{
            if(_camera == null) _camera = Camera.main;
            return _camera;
            }
        }
        
        //cached WaitForSeconds for coroutines, saves on allocations for gc
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float,WaitForSeconds>();
        public static WaitForSeconds GetWait(float time)
        {
            if(WaitDictionary.TryGetValue(time, out var wait)) return wait;
            
            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
        
        //casts a ray to know if the point is over the UI layer or not. a simple check but important
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;
        public static bool IsOverUI(){
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }
        
        //get a 3d world position of a canvas element (will depend on the camera's planes)
        //useful is a 3d object follows a piece of text
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
            return result;
        }

        //destroy everything
        public static void DeleteChildren(this Transform t) //extension method
        {
            foreach(Transform child in t) Object.Destroy(child.gameObject);
        }
        
    }
}