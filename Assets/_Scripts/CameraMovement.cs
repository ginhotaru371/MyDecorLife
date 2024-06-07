using System;
using _Scripts;
using _Scripts.Trash;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera myCamera;

    private Vector3 oriPos;

    private float zoom;
    private float zoomMultiplier = 2.0f;

    private const float minZoom = 1.1f;
    private const float maxZoom = 4.0f;
    private static float velocity = 0f;
    private const float smoothTime = 0.25f;
    private const float minX = 9.5f;
    private const float maxX = 10.0f;
    private const float minY = 7.0f;
    private const float maxY = 7.8f;
    private const float minZ = -7.34f;
    private const float maxZ = -6.98f;


    private void Start()
    {
        zoom = myCamera.orthographicSize;
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
            if (TrashManager.instance.Dragging() || PaintManager.instance.Paintable()) return;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zoom -= scroll * zoomMultiplier;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    
            myCamera.orthographicSize = Mathf.SmoothDamp(myCamera.orthographicSize, zoom, ref velocity, smoothTime);
                
        }
    }

    private void PanCamera()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (TrashManager.instance.Dragging() || PaintManager.instance.Paintable()) return;
            
            var hit = Cast();
            if (hit.collider)
            {
                if (hit.collider.CompareTag("trash") ) return;
                oriPos = myCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (TrashManager.instance.Dragging() || PaintManager.instance.Paintable()) return;
            
            var hit = Cast();
            if (hit.collider)
            {
                if (hit.collider.CompareTag("trash") ) return;
                var diff = oriPos - myCamera.ScreenToWorldPoint(Input.mousePosition);

                myCamera.transform.position = ClampCamera(myCamera.transform.position + diff);
            }
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (TrashManager.instance.Dragging() || PaintManager.instance.Paintable()) return;
            
                var hit = Cast();
                if (hit.collider)
                {
                    if (hit.collider.CompareTag("trash") ) return;
                    oriPos = myCamera.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                if (TrashManager.instance.Dragging() || PaintManager.instance.Paintable()) return;
            
                var hit = Cast();
                if (hit.collider)
                {
                    if (hit.collider.CompareTag("trash") ) return;
                    var diff = oriPos - myCamera.ScreenToWorldPoint(Input.mousePosition);

                    myCamera.transform.position = ClampCamera(myCamera.transform.position + diff);
                }
            }
        }
        
#endif

    }

    private Vector3 ClampCamera(Vector3 targetPos)
    {
        float newX = Mathf.Clamp(targetPos.x, minX, maxX);
        float newY = Mathf.Clamp(targetPos.y, minY, maxY);
        float newZ = Mathf.Clamp(targetPos.z, minZ, maxZ);

        return new Vector3(newX, newY, newZ);
    }
    
    private RaycastHit Cast()
    {
        Ray ray = new Ray();
#if UNITY_EDITOR
        ray = myCamera.ScreenPointToRay(Input.mousePosition);
#endif
#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
                
            ray = myCamera.ScreenPointToRay(touch.position);

        }
#endif
        Physics.Raycast(ray, out var hit, Mathf.Infinity);
        
        return hit;
    }
}
