using System.Collections.Generic;
using _Scripts.Decor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "level", menuName = "Level/New Level", order = 0)]
    public class Level : ScriptableObject
    {
        public GameObject oldRoom;
        [Space]
        public GameObject boxDecoration;
        [Space]
        public List<Furniture> furnitures;
        [Space]
        public List<Decoration> decorations;
        [Space] 
        public List<Wall> wallColor;
        [Space]
        public List<Floor> floorMat;
    }
}