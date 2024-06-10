using UnityEngine;

namespace _Scripts.Decor
{
    public class Decor : Singleton<Decor>
    {
        private Camera mainCamera;
    
        private GameObject _selectedObject;
        private GameObject _box;

        private LayerMask _maskWall;
        private Vector3 _offset;
        private Vector3 _originPos;
        [SerializeField] private Vector3 _target;
    
        private bool _placed;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        
            Init();
        }

        private void Start()
        {
            _originPos = transform.position;
        }

        private void Init()
        {
            mainCamera = Camera.main;
            _maskWall = LayerMask.GetMask("Wall", "Floor");
        }

        private void Update()
        {

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Cast();
            
                if (!_selectedObject)
                {
                    if (hit.collider)
                    {
                        if (!hit.collider.CompareTag("decor")) return;
    
                        CameraMovement.instance.SetBool(false);
                        _selectedObject = hit.collider.gameObject;
                        _offset = _selectedObject.transform.position - GetMousePos();
                        _originPos = _selectedObject.transform.position;
                    }
                }
            }
    
            if (Input.GetMouseButton(0) && _selectedObject)
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out var hit, Mathf.Infinity, _maskWall);

                if (Vector3.Distance(hit.point, _target) < 0.5f)
                {
                    _selectedObject.transform.position = _target;
                    _placed = true;
                }

                if (!_placed)
                {
                    _selectedObject.transform.position = GetMousePos() + _offset;
                    Debug.Log(_selectedObject.transform.position);
                }
            }
    
            if (Input.GetMouseButtonUp(0))
            {
                if (!_selectedObject) return;
                
                CameraMovement.instance.SetBool(true);
                if (_placed)
                {
                    _selectedObject = null;
                    _placed = false;
                }
                else
                {
                    _selectedObject.transform.position = _originPos;
                    _selectedObject = null;
                }
            }
#endif

#if UNITY_ANDROID
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    var hit = Cast();
            
                    if (!_selectedObject)
                    {
                        if (hit.collider)
                        {
                            if (!hit.collider.CompareTag("decor")) return;
    
                            CameraMovement.instance.SetBool(false);
                            _selectedObject = hit.collider.gameObject;
                            _offset = _selectedObject.transform.position - GetMousePos();
                            _originPos = _selectedObject.transform.position;
                        }
                    }
                }
                
                if (touch.phase == TouchPhase.Moved && _selectedObject)
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                    Physics.Raycast(ray, out var hit, Mathf.Infinity, _maskWall);

                    if (Vector3.Distance(hit.point, _target) < 0.5f)
                    {
                        _selectedObject.transform.position = _target;
                        _placed = true;
                    }

                    if (!_placed)
                    {
                        _selectedObject.transform.position = GetMousePos() + _offset;
                        Debug.Log(_selectedObject.transform.position);
                    }
                }
    
                if (touch.phase == TouchPhase.Ended)
                {
                    if (!_selectedObject) return;
                    
                    CameraMovement.instance.SetBool(true);
                    if (_placed)
                    {
                        _selectedObject = null;
                        _placed = false;
                    }
                    else
                    {
                        _selectedObject.transform.position = _originPos;
                        _selectedObject = null;
                    }
                }
                
            }
#endif
            
        }
        
        private RaycastHit Cast()
        {
            Ray ray = new Ray();
#if UNITY_EDITOR
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
#endif
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                ray = mainCamera.ScreenPointToRay(touch.position);

            }
#endif
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
        
            return hit;
        }
        
        private Vector3 GetMousePos()
        {
            Vector3 mousePos = new Vector3();
            
#if UNITY_EDITOR
            mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                mainCamera.WorldToScreenPoint(transform.position).z);
            
#endif

#if UNITY_ANDROID

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                mousePos = new Vector3(touch.position.x, Input.GetTouch(0).position.y,
                    mainCamera.WorldToScreenPoint(transform.position).z);
            }
#endif
            return mainCamera.ScreenToWorldPoint(mousePos);
        }


        public void SetTarget(Vector3 target)
        {
            _target = transform.TransformPoint(target);
            
        }
    }
}
