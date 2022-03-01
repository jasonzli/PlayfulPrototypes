using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OvenFresh
{
    public class TreeOfText : MonoBehaviour
    {
        public TextTreeGenerator Parent;

        public void Init(TextTreeGenerator generator)
        {
            Parent = generator;
        }
        public void Remove()
        {
            Parent.AddToPool(gameObject);
            gameObject.SetActive(false);
        }
        
    }
}