using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts.Trash
{
    public class TrashManager : Singleton<TrashManager>
    {
        [SerializeField] private Camera mainCamera;

        private AsyncOperationHandle<Level> level;
    
        private GameObject _selectedObject;
        private Material _mat;
        private GameObject _box;

        private LayerMask _maskWall;
        private Vector3 _offset;
        private Vector3 _originPos;
    
        private bool _placed;

        private readonly int _color1 = Shader.PropertyToID("_Color");

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        
            Init();
        }

        private void Init()
        {
            mainCamera = Camera.main;
            FindBox();
            _maskWall = LayerMask.GetMask("Wall", "Box");
            level = Addressables.LoadAssetAsync<Level>("level1");
        }

        private void FindBox()
        {
            if (!_box)
            {
                _box = GameObject.FindGameObjectWithTag("box");
            }
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
                        if (!hit.collider.CompareTag("trash")) return;
    
                        _selectedObject = hit.collider.gameObject;
                        _mat = _selectedObject.GetComponent<Renderer>().material;
                        _offset = _selectedObject.transform.position - GetMousePos();
                        _originPos = _selectedObject.transform.position;
                        // BlurObject(0.5f);
                    }
                }
            }
    
            if (Input.GetMouseButton(0) && _selectedObject)
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out var hit, Mathf.Infinity, _maskWall);
            
                if (hit.collider)
                {
                    if (hit.collider.CompareTag("box"))
                    {
                        var newPos = _box.transform.localPosition;
                        newPos.y = newPos.y + 0.02f;
                        _selectedObject.transform.localPosition = newPos;
                        // BlurObject(1.0f);
                        _placed = true;
                    }
                    else
                    {
                        // BlurObject(0.5f);
                        _placed = false;
                    }
                }

                if (!_placed)
                {
                    _selectedObject.transform.position = GetMousePos() + _offset;
                }
            }
    
            if (Input.GetMouseButtonUp(0))
            {
                if (!_selectedObject) return;
                if (_placed)
                {
                    Destroy(_selectedObject.gameObject);
                    _selectedObject = null;
                    _placed = false;
                }
                else
                {
                    // BlurObject(1.0f);
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
                            if (!hit.collider.CompareTag("trash")) return;
    
                            _selectedObject = hit.collider.gameObject;
                            _mat = _selectedObject.GetComponent<Renderer>().material;
                            _offset = _selectedObject.transform.position - GetMousePos();
                            _originPos = _selectedObject.transform.position;
                            // BlurObject(0.5f);
                        }
                    }
                }
                
                if (touch.phase == TouchPhase.Moved && _selectedObject)
                {
                    var ray = mainCamera.ScreenPointToRay(touch.position);

                    Physics.Raycast(ray, out var hit, Mathf.Infinity, _maskWall);
            
                    if (hit.collider)
                    {
                        if (hit.collider.CompareTag("box"))
                        {
                            var newPos = _box.transform.localPosition;
                            newPos.y = newPos.y + 0.02f;
                            _selectedObject.transform.localPosition = newPos;
                            // BlurObject(1.0f);
                            _placed = true;
                        }
                        else
                        {
                            // BlurObject(0.5f);
                            _placed = false;
                        }
                    }

                    if (!_placed)
                    {
                        _selectedObject.transform.position = GetMousePos() + _offset;
                    }
                }
    
                if (touch.phase == TouchPhase.Ended)
                {
                    if (!_selectedObject) return;
                    if (_placed)
                    {
                        Destroy(_selectedObject.gameObject);
                        _selectedObject = null;
                        _placed = false;
                    }
                    else
                    {
                        // BlurObject(1.0f);
                        _selectedObject.transform.position = _originPos;
                        _selectedObject = null;
                    }
                }
            }
#endif
            
            if (TrashAvailableToRemove())
            {
                TrashBox.instance.AvailableToClean();
                enabled = false;
            }
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

        private void BlurObject(float alpha)
        {
            var newColor = _mat.color;
            newColor.a = alpha;
            _mat.SetColor(_color1, newColor);
        }

        private bool TrashAvailableToRemove()
        {
            var removed = true;
            foreach (Transform child in gameObject.transform)
            {
                if (child.CompareTag("trash"))
                {
                    removed = false;
                }
            }

            return removed;
        }

        public void InteriorAvailableToRemove()
        {
            Debug.Log(gameObject.transform.childCount);
            
            if (gameObject.transform.childCount > 1)
            {
                foreach (Transform child in gameObject.transform)
                {
                    if (child.CompareTag("trash_interior"))
                    {
                        child.GetComponent<InteriorRemove>().AvailableToClean();
                    }
                }
            }
            else
            {
                // ButtonGroup.instance.Show(level.Result.wallColor);
                // NewFurniture.instance.SpawnNewFurniture();
                DecorButtonGroup.instance.ShowPainter();
                GameManger.instance.InteriorRemoved();
            }
        }
    }
}
