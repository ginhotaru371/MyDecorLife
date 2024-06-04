using UnityEngine;

namespace _Scripts.Decor
{
    public class Decor : Singleton<Decor>
    {
        [SerializeField] private Camera mainCamera;
    
        private GameObject _selectedObject;
        private GameObject _box;

        private LayerMask _maskWall;
        private Vector3 _offset;
        private Vector3 _originPos;
    
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
            FindBox();
            _maskWall = LayerMask.GetMask("Wall", "Target");
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
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Cast();
            
                if (!_selectedObject)
                {
                    if (hit.collider)
                    {
                        if (!hit.collider.CompareTag("decor")) return;
    
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
            
                var target = new Vector3(-0.274f, 0.77f, 7.739f);

                if (Vector3.Distance(hit.point, target) < 0.2f)
                {
                    _selectedObject.transform.position = target;
                    _placed = true;
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
    
        private RaycastHit Cast()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
        
            return hit;
        }
    
        private Vector3 GetMousePos()
        {
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                mainCamera.WorldToScreenPoint(transform.position).z);
            return mainCamera.ScreenToWorldPoint(mousePos);
        }
    }
}
