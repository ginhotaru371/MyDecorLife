using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Decor
{
    public class NewDecoration : Singleton<NewDecoration>
    {
        [SerializeField] private Level _level;

        private List<GameObject> decorations;

        public List<GameObject> Decorations => decorations;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();

            decorations = new List<GameObject>();
        }

        public void SpawnNewBoxDecoration()
        {
            var oriPos = _level.boxDecoration.transform.position;
            var newPos = new Vector3(oriPos.x, oriPos.y + 1.0f, oriPos.z);
            
            var newBox = Instantiate(_level.boxDecoration, newPos, Quaternion.identity, transform);
            newBox.tag = "box_decor";
            newBox.layer = LayerMask.NameToLayer("Box");

            newBox.transform.DOLocalMove(oriPos, 1.0f).SetEase(Ease.OutBounce);
        }
        //
        // public void SpawnNewDecorations()
        // {
        //     foreach (var child in _level.decorations)
        //     {
        //         var furniture = Instantiate(child.Prefab, this.transform);
        //         furniture.tag = child.Id;
        //         decorations.Add(furniture);
        //
        //         furniture.SetActive(false);
        //     }
        //     
        //     DecorButtonGroup.instance.Show();
        // }
    }
}