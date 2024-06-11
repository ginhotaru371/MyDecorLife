using UnityEngine;

namespace _Scripts.Decor
{
    public class FloorChange : Singleton<FloorChange>
    {
        private static readonly int MainTex = Shader.PropertyToID("_BaseMap");

        private Renderer rend;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();

            rend = GetComponent<MeshRenderer>();
        }
    
        public Transform Change(Texture2D texture)
        {
            rend.material.SetTexture(MainTex, texture);
            Debug.Log("Floor Changed");

            return this.transform;
        }
    }
}
