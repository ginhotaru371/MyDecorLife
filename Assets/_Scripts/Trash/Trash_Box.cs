using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Trash
{
    public class TrashBox : Singleton<TrashBox>
    {
        [SerializeField] private Button btn;
        [SerializeField] private Transform insider;

        private Animator mAnimator;

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
            
            btn.onClick.AddListener(ButtonClicked);
            
            mAnimator = transform.GetChild(3).GetComponent<Animator>();
            
            _newPos = transform.localPosition + dir;
        }

        private void ButtonClicked()
        {
            btn.gameObject.SetActive(false);
            StartCoroutine(Packing());
        }

        public void AvailableToClean()
        {
            StartCoroutine(CloseBox());
        }

        private void Clean()
        {
            btn.gameObject.SetActive(false);
            transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.InBack).OnComplete(()=>
            {
                GameManger.instance.TrashRemoved();
                TrashManager.instance.InteriorAvailableToRemove();
                Destroy(this.gameObject);
            });
        }

        public void Scale()
        {
            const float max = 0.009f;
            var distance = new Vector3(0, max/4, 0);
            var defaultScale = transform.localScale;
            transform.DOScale(defaultScale + new Vector3(0.3f, 0.3f, 0.3f), 0.1f).OnComplete(() =>
            {
                transform.DOScale(defaultScale, 0.2f);
            });

            if (insider.localPosition.y < max)
            {
                insider.DOLocalMove(insider.localPosition + distance, 0.5f);
            }
        }

        private IEnumerator CloseBox()
        {
            if (mAnimator)
            {
                mAnimator.SetTrigger("Full");

                yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);
                
                btn.interactable = true;
                btn.gameObject.SetActive(true);
            }
        }

        public IEnumerator Packing()
        {
            if (mAnimator)
            {
                mAnimator.SetTrigger("Clean");

                yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);
                
                Clean();
            }
        }
    }
}
