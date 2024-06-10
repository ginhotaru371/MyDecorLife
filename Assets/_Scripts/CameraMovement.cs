using System.Collections.Generic;
using _Scripts;
using _Scripts.Trash;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{
    [SerializeField] private List<Camera> myCameras;

    private Vector3 oriPos;

    private float zoom;
    private float zoomMultiplier = 2.0f;

    private const float minZoom = 1.1f;
    private const float maxZoom = 4.0f;
    // private float minX = -0.25f;
    // private float maxX = 0.25f;
    // private float minY = 5.8f;
    // private float maxY = 6.6f;
    // private float minZ = -8.28f;
    // private float maxZ = -7.92f;

    // [SerializeField] private float minMapX, maxMapX, minMapY, maxMapY;

    private bool movable;

    public override void Awake()
    {
        base.KeepAlive(false);
        base.Awake();
    }

    private void Start()
    {
        zoom = myCameras[0].orthographicSize;
    }

    private void Update()
    {
        PanCamera();
        Zoom();
    }

    private void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (!movable || PaintManager.instance.Paintable()) return;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            
            zoom -= scroll * zoomMultiplier;

            foreach (var cam in myCameras)
            {
                cam.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
            }
        }
    }

    private void PanCamera()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!movable || PaintManager.instance.Paintable()) return;
            
            var hit = Cast();
            if (hit.collider)
            {
                if (hit.collider.CompareTag("trash") || hit.collider.CompareTag("decor") || hit.collider.CompareTag("box") || hit.collider.CompareTag("box_decor")) return;
                oriPos = myCameras[0].ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!movable || PaintManager.instance.Paintable()) return;
            
            var hit = Cast();
            if (hit.collider)
            {
                if (hit.collider.CompareTag("trash") || hit.collider.CompareTag("decor") || hit.collider.CompareTag("box") || hit.collider.CompareTag("box_decor")) return;
                var diff = oriPos - myCameras[0].ScreenToWorldPoint(Input.mousePosition);

                foreach (var cam in myCameras)
                {
                    cam.transform.position += diff;
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
                if (!movable || PaintManager.instance.Paintable()) return;
            
                var hit = Cast();
                if (hit.collider)
                {
                    if (hit.collider.CompareTag("trash") || hit.collider.CompareTag("decor") || hit.collider.CompareTag("box") || hit.collider.CompareTag("box_decor") ) return;
                    oriPos = myCameras[0].ScreenToWorldPoint(Input.mousePosition);
                }
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                if (!movable || PaintManager.instance.Paintable()) return;
            
                var hit = Cast();
                if (hit.collider)
                {
                    if (hit.collider.CompareTag("trash") || hit.collider.CompareTag("decor") || hit.collider.CompareTag("box") || hit.collider.CompareTag("box_decor") ) return;
                    var diff = oriPos - myCameras[0].ScreenToWorldPoint(Input.mousePosition);
                   
                    foreach (var cam in myCameras)
                    {
                        cam.transform.position += diff;
                    }
                }
            }
        }
        
#endif

    }

    // private Vector3 ClampCamera(Vector3 targetPos)
    // {
    //     float camHeight = myCamera.orthographicSize;
    //     float camWidth = myCamera.orthographicSize * myCamera.aspect;
    //
    //     float minX = minMapX + camWidth;
    //     float maxX = maxMapX - camWidth;
    //     float minY = minMapY + camHeight;
    //     float maxY = maxMapY - camHeight;
    //     
    //     
    //     float newX = Mathf.Clamp(targetPos.x, minX, maxX);
    //     float newY = Mathf.Clamp(targetPos.y, minY, maxY);
    //     float newZ = Mathf.Clamp(targetPos.z, minZ, maxZ);
    //     
    //     Debug.Log(minX + ", " + maxX + ", " +  minY + ", " +  maxY);
    //     
    //     Debug.Log(newX + ", " + newY + ", " + newZ);
    //
    //     return new Vector3(newX, newY, newZ);
    // }
    
    private RaycastHit Cast()
    {
        Ray ray = new Ray();
#if UNITY_EDITOR
        ray = myCameras[0].ScreenPointToRay(Input.mousePosition);
#endif
#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
                
            ray = myCameras[0].ScreenPointToRay(touch.position);
        }
#endif
        Physics.Raycast(ray, out var hit, Mathf.Infinity);
        
        return hit;
    }

    public bool GetBool()
    {
        return movable;
    }

    public void SetBool(bool m)
    {
        movable = m;
    }
}
