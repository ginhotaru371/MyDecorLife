using _Scripts.Decor;
using _Scripts.UI;
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

        [SerializeField] private bool filled = false;

        private void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
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
                        Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                        
                        transform.position = hit.point;
                        Paintable p = hit.collider.GetComponent<Paintable>();
                        if(ps != null){
                            PaintManager.instance.Paint(p, hit.point, radius, hardness, strength, paintColor);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var percent = ps.CalculateFill(paintColor, 0.5f);
                Debug.Log(percent);

                if (percent > 39.0f)
                {
                    if (!filled)
                    {
                        ButtonGroup.instance.ShowCompleteButton();
                        filled = true;
                    }
                }
            }
#endif

#if UNITY_ANDROID
            
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    Ray ray = mainCamera.ScreenPointToRay(touch.position);
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

                    if (percent > 39.0f)
                    {
                        if (!filled)
                        {
                            ButtonGroup.instance.ShowCompleteButton();
                            filled = true;
                        }
                    }
                }
                
            }
#endif
            
        }

        public void Init()
        {
            var wall = GameObject.FindGameObjectWithTag("wall");
            ps = wall.GetComponent<Paintable>();
        }

        public void ChangePaintColor(Color color)
        {
            if (color == paintColor) return;
            paintColor = color;
            PaintManager.instance.InitTextures(ps);
            filled = false;
        
            PaintManager.instance.Painting(true);
        }

        public void CheckFill()
        {
            filled = false;
            
            var percent = ps.CalculateFill(paintColor, 0.5f);

            if (percent > 39.0f)
            {
                if (!filled)
                {
                    ButtonGroup.instance.ShowCompleteButton();
                    filled = true;
                }
            }
        }
    }
}
