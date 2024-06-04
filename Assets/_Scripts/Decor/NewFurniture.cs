using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Decor
{
    public class NewFurniture : Singleton<NewFurniture>
    {
        [SerializeField] private Level _level;

        private List<GameObject> _furnitures;

        public List<GameObject> Furniture => _furnitures;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();

            _furnitures = new List<GameObject>();
        }

        public void SpawnNewFurniture()
        {
            foreach (var child in _level.furnitures)
            {
                var furniture = Instantiate(child.Prefab, this.transform);
                furniture.tag = child.Id;
                _furnitures.Add(furniture);

                furniture.SetActive(false);
            }
            
            DecorButtonGroup.instance.Show();
        }
    }
}
