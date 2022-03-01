using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    public class TextTreeGenerator : MonoBehaviour
    {

        public GameObject TreeObject;
        public float TreeLifeTime = 1.0f;
        public float TreeSpeed = 1.0f;
        public float TreeDistanceLimit = 10f;
        

        private List<GameObject> _AllTrees;
        private Stack<GameObject> _TreePool;
        
        void Start()
        {
            _AllTrees = new List<GameObject>();
        }

        void Update()
        {
            
        }

        private GameObject NewTree(Vector3 position, Quaternion orientation, Transform parent)
        {
            
        }
    }
}
