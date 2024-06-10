using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Decor
{
    public class DecorBox : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField] private GameObject insider;
        private float distance;
        
        [SerializeField] private List<Decoration> prefabs;

        private GameObject _decor;

        private Vector3 _newPos;

        private Animator mAnimator;

        private bool knifeDone, animationDone;
        private bool open1Done, open2Done, open3Done, open4Done;

        private void Start()
        {
            mainCamera = Camera.main;
            prefabs = new List<Decoration>(Addressables.LoadAssetAsync<Level>("level1").Result.decorations);
            distance = insider.transform.localPosition.z / prefabs.Count;
            mAnimator = transform.GetChild(0).GetComponent<Animator>();
        }

        private void Update()
        {
            
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("box_decor"))
                {
                    if (!knifeDone)
                    {
                        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        
                        StartCoroutine(OpenBox("Knife"));
                        knifeDone = true;
                        return;
                    }
                    if (!open1Done)
                    {
                        StartCoroutine(OpenBox("Open1"));
                        open1Done = true;
                        return;
                    }
                    if (!open2Done)
                    {
                        StartCoroutine(OpenBox("Open2"));
                        open2Done = true;
                        return;
                    }
                    if (!open3Done)
                    {
                        StartCoroutine(OpenBox("Open3"));
                        open3Done = true;
                        return;
                    }
                    if (!open4Done)
                    {
                        StartCoroutine(OpenBox("Open4"));
                        open4Done = true;
                        return;
                    }

                    if (!animationDone) return;
                    
                    if (!_decor)
                    {
                        if (prefabs.Count > 0)
                        {
                            var oriPos = prefabs[0].Prefab.transform.position;
                            Decor.instance.SetTarget(oriPos);

                            var oriRot = prefabs[0].Prefab.transform.rotation;
                            
                            _decor = Instantiate(prefabs[0].Prefab, transform.parent);
                            _decor.transform.position = transform.position;
                            prefabs.RemoveAt(0);

                            _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
                            InsiderRemove();
                        }
                        else
                        {
                            StartCoroutine(OpenBox("Hide"));
                        }

                    }
                    else
                    {
                        if (prefabs.Count > 0)
                        {
                            Debug.Log(prefabs.Count);
                            if (_decor.transform.localPosition != _newPos)
                            {
                                _decor = null;
                                var oriPos = prefabs[0].Prefab.transform.position;
                                Decor.instance.SetTarget(oriPos);
                            
                                var oriRot = prefabs[0].Prefab.transform.rotation;
                            
                                _decor = Instantiate(prefabs[0].Prefab, transform.parent);
                                _decor.transform.position = transform.position;
                                prefabs.RemoveAt(0);
                            
                                _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
                                InsiderRemove();
                            }
                        }
                        else
                        {
                            Debug.Log(prefabs.Count);
                            StartCoroutine(OpenBox("Hide"));
                        }
                    }
                }
            }
#endif
            
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = mainCamera.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("box_decor"))
                    {
                        if (!knifeDone)
                        {
                            StartCoroutine(OpenBox("Knife"));
                            return;
                        }
                        if (!open1Done)
                        {
                            StartCoroutine(OpenBox("Open1"));
                            return;
                        }
                        if (!open2Done)
                        {
                            StartCoroutine(OpenBox("Open2"));
                            return;
                        }
                        if (!open3Done)
                        {
                            StartCoroutine(OpenBox("Open3"));
                            return;
                        }
                        if (!open4Done)
                        {
                            StartCoroutine(OpenBox("Open4"));
                            return;
                        }
                        
                        if (!_decor)
                        {
                            if (prefabs.Count > 0)
                            {
                                var oriPos = prefabs[0].Prefab.transform.position;
                                Decor.instance.SetTarget(oriPos);
                                var oriRot = prefabs[0].Prefab.transform.rotation;
                                
                                _decor = Instantiate(prefabs[0].Prefab, transform.parent);
                                _decor.transform.position = transform.position;
                                prefabs.RemoveAt(0);

                                _decor.transform.DOLocalMoveY(_newPos.y, 0.5f).SetEase(Ease.OutBack);
                                InsiderRemove();
                            }
                            else
                            {
                                StartCoroutine(OpenBox("Hide"));
                            }
                        }
                        else
                        {
                            if (_decor.transform.localPosition != _newPos && prefabs.Count > 0)
                            {
                                _decor = null;
                                
                                var oriPos = prefabs[0].Prefab.transform.position;
                                Decor.instance.SetTarget(oriPos);
                                var oriRot = prefabs[0].Prefab.transform.rotation;
                                
                                _decor = Instantiate(prefabs[0].Prefab, transform.parent);
                                _decor.transform.position = transform.position;
                                prefabs.RemoveAt(0);

                                _decor.transform.DOLocalMoveY(_newPos.y, 0.5f).SetEase(Ease.OutBack);
                                InsiderRemove();
                            }
                            else
                            {
                                StartCoroutine(OpenBox("Hide"));
                            }
                        }
                    }
                }
            }
#endif
            
        }

        public void SetDecorationSpawnPos(Vector3 newPos)
        {
            _newPos = newPos;
            _newPos.y += 0.04f;
        }

        private IEnumerator OpenBox(string trigger)
        {
            if (mAnimator)
            {
                if (trigger == "Knife")
                {
                    mAnimator.SetTrigger(trigger);
                    yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);

                    knifeDone = true;
                    // open1Done = false;
                }
                else if (trigger == "Open1")
                {
                    mAnimator.SetTrigger(trigger);
                    yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);

                    open1Done = true;
                    // open2Done = false;
                }
                else if (trigger == "Open2")
                {
                    mAnimator.SetTrigger(trigger);
                    yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);

                    open2Done = true;
                    // open3Done = false;
                }
                else if (trigger == "Open3")
                {
                    mAnimator.SetTrigger(trigger);
                    yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);

                    open3Done = true;
                    // open4Done = false;
                }
                else if (trigger == "Open4")
                {
                    mAnimator.SetTrigger(trigger);
                    yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);

                    open4Done = true;
                    animationDone = true;
                }
                else if (trigger == "Hide")
                {
                    mAnimator.SetTrigger(trigger);
                    
                    yield return new WaitForSeconds(mAnimator.GetCurrentAnimatorStateInfo(0).length);
                    
                    transform.DOLocalMoveX(-0.5f, 0.5f).SetEase(Ease.InBack).OnComplete(()=>
                    {
                        Destroy(this.gameObject);
                    });
                }
            }
        }

        private void InsiderRemove()
        {
            insider.transform.DOLocalMoveZ(insider.transform.localPosition.z - distance, 0.5f);
        }
    }
}
