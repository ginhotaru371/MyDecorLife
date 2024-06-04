using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class MousePainter : Singleton<MousePainter>
    {
        [SerializeField] private Camera mainCamera;
    
        public Color paintColor;
    
        public float radius = 1;
        public float strength = 1;
        public float hardness = 1;
    
        [SerializeField] private Paintable ps;

        private void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        }

        private void Start()
        {
            Init();
        }

        void Update()
        {
            if (!PaintManager.instance.Paintable()) return;

#if UNITY_EDITOR
            if (Input.GetMouseButton(0)){
                
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100.0f)){

                    if (hit.collider)
                    {
                        if (hit.collider.CompareTag("wall"))
                        {
                            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                        
                            transform.position = hit.point;
                            Paintable p = hit.collider.GetComponent<Paintable>();
                            if(ps != null){
                                PaintManager.instance.Paint(p, hit.point, radius, hardness, strength, paintColor);
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var percent = ps.CalculateFill(paintColor, 0.5f);
                Debug.Log(percent);

                if (percent > 115)
                {
                    PaintManager.instance.Painting(false);
                }
            }
#endif

#if UNITY_ANDROID
            
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100.0f)){

                        if (hit.collider)
                        {
                            if (hit.collider.CompareTag("wall"))
                            {
                                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                        
                                transform.position = hit.point;
                                Paintable p = hit.collider.GetComponent<Paintable>();
                                if(ps != null){
                                    PaintManager.instance.Paint(p, hit.point, radius, hardness, strength, paintColor);
                                }
                            }
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    var percent = ps.CalculateFill(paintColor, 0.5f);
                    Debug.Log(percent);

                    if (percent > 115)
                    {
                        PaintManager.instance.Painting(false);
                    }
                }
                
            }
#endif
            
        }

        private void Init()
        {
            // foreach (Transform child in transform.parent)
            // {
            //     if (child.CompareTag("wall"))
            //     {
            //         ps.Add(child.GetComponent<Paintable>());
            //     }
            // }

            var wall = GameObject.FindGameObjectWithTag("wall");
            ps = wall.GetComponent<Paintable>();

        }

        public void ChangePaintColor(Color color)
        {
            if (color == paintColor) return;
            paintColor = color;
            //
            // foreach (var child in ps)
            // {
            //     PaintManager.instance.InitTextures(child);
            // }
            PaintManager.instance.InitTextures(ps);
        
            PaintManager.instance.Painting(true);
        }
    }
}
