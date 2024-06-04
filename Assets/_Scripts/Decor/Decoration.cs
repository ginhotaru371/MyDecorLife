using System;
using UnityEngine;

namespace _Scripts.Decor
{
    [Serializable]
    
    public class Decoration
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector3 position;

        public GameObject Prefab => prefab;
        public Vector3 Position => position;
    }
}