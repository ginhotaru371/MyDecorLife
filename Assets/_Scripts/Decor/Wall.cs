using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Decor
{
    
    [Serializable]
    
    public class Wall
    {
        [SerializeField] private Color _color;
        [SerializeField] private Sprite _image;

        public Color Color => _color;
        public Sprite Image => _image;
    }
}