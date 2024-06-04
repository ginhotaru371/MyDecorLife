using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Trash
{
    public class TrashBox : Singleton<TrashBox>
    {
        private Button btn;

        private Vector3 _newPos;

        [SerializeField] private Vector3 dir;
    
        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        
            Init();
        }

        private void Init()
        {
            btn = transform.GetChild(0).Find("Clean").GetComponent<Button>();
            btn.interactable = false;
            btn.gameObject.SetActive(false);
            
            _newPos = transform.localPosition + dir;
        }

        public void AvailableToClean()
        {
            btn.interactable = true;
            btn.gameObject.SetActive(true);
        }

        public void Clean()
        {
            btn.gameObject.SetActive(false);
            transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.InOutBack).OnComplete(()=>
            {
                GameManger.instance.TrashRemoved();
                TrashManager.instance.InteriorAvailableToRemove();
                Destroy(this.gameObject);
            });
            
        }
    }
}
