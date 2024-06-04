using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Decor
{
    public class DecorBox : MonoBehaviour
    {
        private Camera mainCamera;
        
        public GameObject prefab;

        private GameObject _decor;

        private Vector3 _newPos;

        private void Start()
        {
            mainCamera = Camera.main;
            _newPos = transform.localPosition;
            _newPos.y += 0.02f;
        }

        private void Update()
        {
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
                            _decor = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);

                            _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
                        }
                        else
                        {
                            if (_decor.transform.localPosition != _newPos)
                            {
                                _decor = null;
                                _decor = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);

                                _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
                            }
                        }
                    }
                }
            }
#endif

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("box_decor"))
                {
                    if (!_decor)
                    {
                        _decor = Instantiate(prefab, transform.localPosition, Quaternion.identity, transform.parent);

                        _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
                    }
                    else
                    {
                        if (_decor.transform.localPosition != _newPos)
                        {
                            _decor = null;
                            _decor = Instantiate(prefab, transform.localPosition, Quaternion.identity, transform.parent);

                            _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
                        }
                    }
                }
            }
#endif
        }

        // private void OnMouseDown()
        // {
        //     if (!_decor)
        //     {
        //         _decor = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
        //
        //         _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
        //     }
        //     else
        //     {
        //         if (_decor.transform.localPosition != _newPos)
        //         {
        //             _decor = null;
        //             _decor = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
        //
        //             _decor.transform.DOLocalMove(_newPos, 0.5f).SetEase(Ease.OutBack);
        //         }
        //     }
        // }
    }
}
