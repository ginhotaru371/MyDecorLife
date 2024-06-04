using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Decor
{
    [Serializable]
    
    public class Furniture
    {
        [SerializeField] private string id;
        [SerializeField] private Type type;
        [SerializeField] private Sprite image;
        [SerializeField] private GameObject prefab;

        public string Id => id;
        public Type Type => type;
        public Sprite Image => image;
        public GameObject Prefab => prefab;
    }
}