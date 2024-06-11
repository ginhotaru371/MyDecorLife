using System.Collections.Generic;
using _Scripts.UI;
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
            var newPos = transform.TransformPoint(oriPos);
            newPos.y += 2.0f;
            
            var newBox = Instantiate(_level.boxDecoration, transform);
            newBox.transform.position = newPos;
            newBox.layer = LayerMask.NameToLayer("Box");

            newBox.transform.DOLocalMove(oriPos, 1.0f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                newBox.GetComponent<DecorBox>().SetDecorationSpawnPos(newBox.transform.localPosition);
            });
            
            ButtonGroup.instance.HideCompleteButton();
        }
    }
}