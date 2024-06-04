using UnityEngine;

namespace _Scripts.Decor
{
    public class FloorChange : Singleton<FloorChange>
    {
        private static readonly int BaseColorMap = Shader.PropertyToID("_BASE_COLOR_MAP");

        private Renderer rend;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();

            rend = GetComponent<MeshRenderer>();
        }
    
        public void Change(Texture2D texture)
        {
            rend.material.SetTexture(BaseColorMap, texture);
            Debug.Log("Floor Changed");
        }
    }
}
