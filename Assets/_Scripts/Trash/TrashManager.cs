using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts.Trash
{
    public class TrashManager : Singleton<TrashManager>
    {
        [SerializeField] private Camera mainCamera;

        private AsyncOperationHandle<Level> level;

        private const float NormalAlpha = 1.0f;
        private const float BlurAlpha = 0.7f;
    
        private GameObject selectedObject;
        private Renderer rend;
        private Material mat;
        private GameObject box;

        private LayerMask maskWall;
        private Vector3 offset;
        private Vector3 originPos;
    
        private bool placed;
        private bool dragging;

        private readonly int opacity = Shader.PropertyToID("_Opacity");

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
            maskWall = LayerMask.GetMask("Wall", "Box");
            level = Addressables.LoadAssetAsync<Level>("level1");
        }

        private void FindBox()
        {
            if (!box)
            {
                box = GameObject.FindGameObjectWithTag("box");
            }
        }

        private void Update()
        {

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Cast();
            
                if (!selectedObject)
                {
                    if (hit.collider)
                    {
                        if (!hit.collider.CompareTag("trash")) return;

                        dragging = true;
                        selectedObject = hit.collider.gameObject;
                        rend = selectedObject.GetComponent<Renderer>();
                        offset = selectedObject.transform.position - GetMousePos();
                        originPos = selectedObject.transform.position;
                        BlurObject(BlurAlpha);
                    }
                }
            }
    
            if (Input.GetMouseButton(0) && selectedObject)
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out var hit, Mathf.Infinity, maskWall);
            
                if (hit.collider)
                {
                    if (hit.collider.CompareTag("box"))
                    {
                        var newPos = box.transform.localPosition;
                        newPos.y = newPos.y + 0.08f;
                        selectedObject.transform.localPosition = newPos;
                        BlurObject(NormalAlpha);
                        placed = true;
                    }
                    else
                    {
                        BlurObject(BlurAlpha);
                        placed = false;
                    }
                }

                if (!placed)
                {
                    selectedObject.transform.position = GetMousePos() + offset;
                }
            }
    
            if (Input.GetMouseButtonUp(0))
            {
                if (!selectedObject) return;

                dragging = false;
                if (placed)
                {
                    Destroy(selectedObject.gameObject);
                    TrashBox.instance.Scale();
                    selectedObject = null;
                    placed = false;
                }
                else
                {
                    BlurObject(NormalAlpha);
                    selectedObject.transform.position = originPos;
                    selectedObject = null;
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
            
                    if (!selectedObject)
                    {
                        if (hit.collider)
                        {
                            if (!hit.collider.CompareTag("trash")) return;

                            dragging = true;
                            selectedObject = hit.collider.gameObject;
                            rend = selectedObject.GetComponent<Renderer>();
                            offset = selectedObject.transform.position - GetMousePos();
                            originPos = selectedObject.transform.position;
                            BlurObject(BlurAlpha);
                        }
                    }
                }
                
                if (touch.phase == TouchPhase.Moved && selectedObject)
                {
                    var ray = mainCamera.ScreenPointToRay(touch.position);

                    Physics.Raycast(ray, out var hit, Mathf.Infinity, maskWall);
            
                    if (hit.collider)
                    {
                        if (hit.collider.CompareTag("box"))
                        {
                            var newPos = box.transform.localPosition;
                            newPos.y = newPos.y + 0.02f;
                            selectedObject.transform.localPosition = newPos;
                            BlurObject(NormalAlpha);
                            placed = true;
                        }
                        else
                        {
                            BlurObject(BlurAlpha);
                            placed = false;
                        }
                    }

                    if (!placed)
                    {
                        selectedObject.transform.position = GetMousePos() + offset;
                    }
                }
    
                if (touch.phase == TouchPhase.Ended)
                {
                    if (!selectedObject) return;

                    dragging = false;
                    if (placed)
                    {
                        Destroy(selectedObject.gameObject);
                        selectedObject = null;
                        placed = false;
                    }
                    else
                    {
                        BlurObject(NormalAlpha);
                        selectedObject.transform.position = originPos;
                        selectedObject = null;
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
           rend.material.SetFloat(opacity, alpha);
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

        public bool Dragging()
        {
            return dragging;
        }
    }
}
