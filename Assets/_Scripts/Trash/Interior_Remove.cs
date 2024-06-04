using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Trash
{
    public class InteriorRemove : MonoBehaviour
    {
    
        private Button btn;
    
        private Vector3 _newPos;
        [SerializeField] private Vector3 dir;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            btn = transform.GetChild(0).Find("Clean").GetComponent<Button>();
        
            _newPos = transform.localPosition + dir;
            btn.interactable = false;
            btn.gameObject.SetActive(false);
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
                TrashManager.instance.InteriorAvailableToRemove();
                Destroy(gameObject);
            });
        }
    }
}
