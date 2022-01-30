using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        [SerializeField] private TextMeshProUGUI _textArea;
        
        private DualGrid _dualGrid;
        [SerializeField] 
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
            _dualGrid.ScaleOut(new Vector3(0,0,0), 0f);
            _dualGrid.ScaleOut(new Vector3(1,1,1), 2.2f);
            _textArea.text = _dualGrid.dualConfig.UIText;
            //_dualGrid.SpinInPlace(2.2f);
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

            StartCoroutine(LevelCompletedRoutine());
        }

        private IEnumerator LevelCompletedRoutine()
        {
            
            _dualGrid.MoveTowards(new Vector3(0,0,-22),1.2f);
            yield return new WaitForSeconds(1.3f);
            
            //set new level
            _dualGrid.UpdateDualGridData(levelList[_level]);
            
            //create new level
            _dualGrid.transform.position = Vector3.zero;
            _dualGrid.Reset(); //reset needs the scale to be 1
            
            //use the coroutine to set the value instantly
            _dualGrid.ScaleOut(new Vector3(0,0,0), 0f);
            _dualGrid.ScaleOut(new Vector3(1,1,1), 2.2f);

            //_dualGrid.SpinInPlace(2.2f);
            _textArea.text = _dualGrid.dualConfig.UIText;
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
