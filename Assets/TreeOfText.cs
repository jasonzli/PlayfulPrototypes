using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OvenFresh
{
    public class TreeOfText : MonoBehaviour
    {
        public TextTreeGenerator Parent;
        public float MovementSpeed;

        public void Init(TextTreeGenerator generator)
        {
            Parent = generator;
            MovementSpeed = generator.TreeSpeed;
        }
        public void Remove()
        {
            Parent.AddToPool(gameObject);
            gameObject.SetActive(false);
        }

        void Update()
        {
            transform.position += -Vector3.forward * MovementSpeed;
            if (transform.position.z < 0f) Remove();
        }
        
        
    }
}