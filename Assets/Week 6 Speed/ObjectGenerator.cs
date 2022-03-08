using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    public class ObjectGenerator : MonoBehaviour
    {
        public FloatParameter WorldSpeed;
        [Tooltip("Production interval in milliseconds")] public FloatParameter GenerationInterval;
        public List<GameObject> ObstacleObjects = new List<GameObject>();

        private List<GameObject> _allObstacles = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GenerateObstacle(GenerationInterval.value));
        }

        IEnumerator GenerateObstacle(float intervalTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalTime);
                //create the thing
                var obstacle = CreateRandomObstacle(ObstacleObjects);
                _allObstacles.Add(obstacle);
            }
            
        }

        GameObject CreateRandomObstacle(List<GameObject> objectsList)
        {
            int index = Random.Range(0, objectsList.Count);
            GameObject obstacle = Instantiate(objectsList[index],transform);
            obstacle.GetComponent<FlyingObject>().Init(WorldSpeed);
            return obstacle;
        }
        
    }
}
