using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace OvenFresh
{
    public class TextTreeGenerator : MonoBehaviour
    {

        public GameObject TreeObject;
        public float TreeLifeTime = 1.0f;
        public float TreeSpeed = 1.0f;
        public float TreeDistanceLimit = 10f;
        

        private List<GameObject> _AllTrees = new List<GameObject>();
        private Stack<GameObject> _TreePool = new Stack<GameObject>();
        
        void Start()
        {
            _AllTrees = new List<GameObject>();
            for (var i = 0; i < 10; i++)
            {
                _AllTrees.Add(NewTree(transform.position + Vector3.right * i, Quaternion.identity, transform));
            }

            StartCoroutine(DestroyAllTreesIn(3));
        }

        IEnumerator DestroyAllTreesIn(float destroyInSeconds)
        {
            var elapsedTime = 0f;
            while (elapsedTime < destroyInSeconds)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foreach (GameObject tree in _AllTrees)
            {
                tree.GetComponent<TreeOfText>().Remove();
            }

            _AllTrees.Clear();
            StartCoroutine(CreateTrees(Random.Range(3,25)));
        }

        IEnumerator CreateTrees(int numberOfTrees)
        {
            for (var i = 0; i < numberOfTrees; i++)
            {
                _AllTrees.Add(NewTree(transform.position + Vector3.right * i, Quaternion.identity, transform));
            }
            StartCoroutine(DestroyAllTreesIn(3));
            yield return null;
        }

        void Update()
        {
            
        }

        public void AddToPool(GameObject tree)
        {
            _TreePool.Push(tree);
        }
        
        private GameObject NewTree(Vector3 position, Quaternion orientation, Transform parent)
        {
            GameObject newTree;
            if (_TreePool.Count > 0)
            {
                newTree = _TreePool.Pop();
                newTree.SetActive(true);
            }
            else
            {
                newTree = Instantiate(TreeObject);
                newTree.GetComponent<TreeOfText>().Init(this);
            }
            
            newTree.transform.position = position;
            newTree.transform.localRotation = orientation;
            newTree.transform.parent = parent;
            
            return newTree;
        }
    }
}
