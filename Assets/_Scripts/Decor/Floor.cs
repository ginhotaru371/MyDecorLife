using System;
using UnityEngine;

namespace _Scripts.Decor
{
    [Serializable]
    public class Floor
    {
        [SerializeField] private Texture2D _texture;
        [SerializeField] private Sprite _image;

        public Texture2D Texture => _texture;
        public Sprite Image => _image;
    }
}