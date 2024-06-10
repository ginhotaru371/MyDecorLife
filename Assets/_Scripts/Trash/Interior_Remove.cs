using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace _Scripts.Trash
{
    public class InteriorRemove : MonoBehaviour
    {
        private Button btn;

        private AsyncOperationHandle<GameObject> fxProof;
        private Vector3 _newPos;
        [SerializeField] private Vector3 dir;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            btn = transform.GetChild(0).Find("Clean").GetComponent<Button>();
            fxProof = Addressables.LoadAssetAsync<GameObject>("FxPoofForHome");
            
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

            var fxProofObject = Instantiate(fxProof.Result, transform.position, Quaternion.identity);
            fxProofObject.GetComponent<ParticleSystem>().Play();
            transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.InBack).OnComplete(()=>
            {
                TrashManager.instance.InteriorAvailableToRemove();
                Destroy(fxProofObject);
                Destroy(gameObject);
            });
        }
    }
}
