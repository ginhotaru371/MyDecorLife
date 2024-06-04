using System;
using UnityEngine;

namespace _Scripts
{
    public class Paintable : MonoBehaviour
    {
        const int TextureSize = 1024;
    
        private RenderTexture _extendIslandsRenderTexture;
        private RenderTexture _uvIslandsRenderTexture;
        private RenderTexture _maskRenderTexture;
        private RenderTexture _supportTexture;
    
        private Renderer _rend;
        private Material _mat;

        private int _maskTextureID = Shader.PropertyToID("_MaskTex");

        public RenderTexture GetMask() => _maskRenderTexture;
        public RenderTexture GetUVIslands() => _uvIslandsRenderTexture;
        public RenderTexture GetExtend() => _extendIslandsRenderTexture;
        public RenderTexture GetSupport() => _supportTexture;
        public Renderer GetRenderer() => _rend;

        private void Start()
        {
            CreateTextures();
            PaintManager.instance.InitTextures(this);
        }

        public void CreateTextures()
        {
            _maskRenderTexture = new RenderTexture(TextureSize, TextureSize, 0);
            _maskRenderTexture.filterMode = FilterMode.Bilinear;

            _extendIslandsRenderTexture = new RenderTexture(TextureSize, TextureSize, 0);
            _extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

            _uvIslandsRenderTexture = new RenderTexture(TextureSize, TextureSize, 0);
            _uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

            _supportTexture = new RenderTexture(TextureSize, TextureSize, 0);
            _supportTexture.filterMode =  FilterMode.Bilinear;

            _rend = GetComponent<Renderer>();
            _rend.material.SetTexture(_maskTextureID, _extendIslandsRenderTexture);
        }
    
        public float CalculateFill (Color reference , float tolerance )
        {
            var texture = _rend.material.GetTexture(_maskTextureID);

            var texture2D = ToTexture2D(texture);

            var colors = texture2D?.GetPixels();
        
        
            Vector3 target = new Vector3{ x=reference.r , y=reference.g , z=reference.b };
            int numHits = 0;
            const float sqrt3 = 1.73205080757f;
            for( int i=0 ; i<colors.Length ; i++ )
            {
                Vector3 next = new Vector3{ x=colors[i].r , y=colors[i].g , z=colors[i].b };
                float mag = Vector3.Magnitude( target - next ) / sqrt3;
                if (mag <= tolerance)
                    numHits += 1;
                else
                    numHits += 0;
            }
            return ((float)numHits / (float)colors.Length) * 100;
        }
    
        public static Texture2D ToTexture2D(Texture texture)
        {
            Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
 
            RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 0);
            Graphics.Blit(texture, renderTexture);
 
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
        
            return texture2D;
        }
    
        void OnDisable(){
            _maskRenderTexture?.Release();
            _uvIslandsRenderTexture?.Release();
            _extendIslandsRenderTexture?.Release();
            _supportTexture?.Release();
        }
    }
}