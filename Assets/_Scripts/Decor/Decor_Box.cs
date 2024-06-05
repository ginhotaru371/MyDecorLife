using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Decor
{
    public class DecorBox : MonoBehaviour
    {
        private Camera mainCamera;
        
        private List<Decoration> prefabs;

        private GameObject _decor;

        private Vector3 _newPos;

        private void Start()
        {
            mainCamera = Camera.main;
            prefabs = new List<Decoration>(Addressables.LoadAssetAsync<Level>("level1").Result.decorations);
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
                    if (!_decor)
                    {
                        if (prefabs.Count > 0)
                        {
                            var oriPos = prefabs[0].Prefab.transform.position;
                            Decor.instance.SetTarget(oriPos);

                            var oriRot = prefabs[0].Prefab.transform.rotation;
                            
                            _decor = Instantiate(prefabs[0].Prefab, transform.position, oriRot, transform.parent);
                            prefabs.RemoveAt(0);

                            _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
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
                            
                            _decor = Instantiate(prefabs[0].Prefab, transform.position, oriRot, transform.parent);
                            prefabs.RemoveAt(0);
                            
                            _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
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
                        if (!_decor)
                        {
                            if (prefabs.Count > 0)
                            {
                                var oriPos = prefabs[0].Prefab.transform.position;
                                Decor.instance.SetTarget(oriPos);
                                var oriRot = prefabs[0].Prefab.transform.rotation;
                                
                                _decor = Instantiate(prefabs[0].Prefab, transform.position, oriRot, transform.parent);
                                prefabs.RemoveAt(0);

                                _decor.transform.DOLocalMoveY(_newPos.y, 0.5f).SetEase(Ease.OutBack);
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
                                
                                _decor = Instantiate(prefabs[0].Prefab, transform.position, oriRot, transform.parent);
                                prefabs.RemoveAt(0);

                                _decor.transform.DOLocalMoveY(_newPos.y, 0.5f).SetEase(Ease.OutBack);
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
    }
}
