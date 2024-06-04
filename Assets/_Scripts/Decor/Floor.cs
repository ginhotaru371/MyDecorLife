using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Decor
{
    [Serializable]
    public class Floor
    {
        [SerializeField] private Material _mat;
        [SerializeField] private Sprite _image;

        public Material Mat => _mat;
        public Sprite Image => _image;
    }
}