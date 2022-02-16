using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace OvenFresh
{
    public class BeatControl : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        void OnReset(InputValue value)
        {
            SceneManager.LoadSceneAsync(0);
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
