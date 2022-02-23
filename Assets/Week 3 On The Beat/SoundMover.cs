using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace OvenFresh
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundMover : MonoBehaviour
    {

        [SerializeField] private AudioSource SoundEffect;
        [Tooltip("Rate in milliseconds")]
        [SerializeField] private float PlayRate = 600;
        
        // Start is called before the first frame update
        void Start()
        {
            SoundEffect = GetComponent<AudioSource>();
            StartCoroutine(Play(PlayRate));
        }

        IEnumerator Play(float beatTiming)
        {
            while (true)
            {
                SoundEffect.Stop();
                SoundEffect.Play();
                yield return new WaitForSeconds(PlayRate*.001f);
            }
        }

        private async Task Operate()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
