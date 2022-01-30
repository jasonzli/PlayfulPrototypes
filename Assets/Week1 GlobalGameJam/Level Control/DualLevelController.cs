using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    //Level controller loads up new levels between each goal
    //it tells the dual grid to update its data
    
    [RequireComponent(typeof(DualGrid))]
    public class DualLevelController : MonoBehaviour
    {
        [SerializeField] private List<DualGridConfiguration> levelList;
        private int _level;

        private DualGrid _dualGrid;
        public int Level
        {
            get { return _level + 1; }
        }

        void Awake()
        {
            _dualGrid = GetComponent<DualGrid>();
        }

        void Start()
        {
            LoadLevel(0);
        }
        void LoadLevel(int index)
        {
            _level = Mathf.Clamp(index,0,levelList.Count-1);
            _dualGrid.UpdateDualGridData(levelList[_level]);
            _dualGrid.Reset();
        }

        void IncrementLevel()
        {
            _level = (_level + 1) % levelList.Count;
        }

        void BackLevel()
        {
            _level = Mathf.Clamp(_level - 1, 0, levelList.Count - 1);
        }

        void LevelCompleted()
        {
            IncrementLevel();
            //Scale out level

            //set new level
            _dualGrid.UpdateDualGridData(levelList[_level]);
            
            //create new level
            _dualGrid.Reset();
        }
        
        void OnEnable()
        {
            DualGrid.GoalReached += LevelCompleted;
        }

        void OnDisable()
        {
            DualGrid.GoalReached -= LevelCompleted;
        }
        
        
    }
}
