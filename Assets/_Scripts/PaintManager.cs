using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts
{
    public class PaintManager : Singleton<PaintManager>
    {
        public Shader texturePaint;

        private readonly int _prepareUvID = Shader.PropertyToID("_PrepareUV");
        private readonly int _positionID = Shader.PropertyToID("_PainterPosition");
        private readonly int _hardnessID = Shader.PropertyToID("_Hardness");
        private readonly int _strengthID = Shader.PropertyToID("_Strength");
        private readonly int _radiusID = Shader.PropertyToID("_Radius");
        private readonly int _colorID = Shader.PropertyToID("_PainterColor");
        private readonly int _textureID = Shader.PropertyToID("_MainTex");

        private Material _paintMaterial;

        private CommandBuffer _command;

        [SerializeField] private bool _paintable;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        
            Init();
        }

        private void Init()
        {
            _paintable = false;
            _paintMaterial = new Material(texturePaint);
            _command = new CommandBuffer();
            _command.name = "CommmandBuffer - " + gameObject.name;
        }

        public void InitTextures(Paintable paintable){
            // paintable.CreateTextures();
        
            var mask = paintable.GetMask();
            var uvIslands = paintable.GetUVIslands();
            var extend = paintable.GetExtend();
            var support = paintable.GetSupport();
            var rend = paintable.GetRenderer();

            _command.SetRenderTarget(mask);
            _command.SetRenderTarget(extend);
            _command.SetRenderTarget(support);

            _paintMaterial.SetFloat(_prepareUvID, 1);
            _command.SetRenderTarget(uvIslands);
            _command.DrawRenderer(rend, _paintMaterial, 0);

            Graphics.ExecuteCommandBuffer(_command);
            _command.Clear();
        }


        public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null){
        
            var mask = paintable.GetMask();
            var uvIslands = paintable.GetUVIslands();
            var extend = paintable.GetExtend();
            var support = paintable.GetSupport();
            var rend = paintable.GetRenderer();

            _paintMaterial.SetFloat(_prepareUvID, 0);
            _paintMaterial.SetVector(_positionID, pos);
            _paintMaterial.SetFloat(_hardnessID, hardness);
            _paintMaterial.SetFloat(_strengthID, strength);
            _paintMaterial.SetFloat(_radiusID, radius);
            _paintMaterial.SetTexture(_textureID, support);
            _paintMaterial.SetColor(_colorID, color ?? Color.red);

            _command.SetRenderTarget(mask);
            _command.DrawRenderer(rend, _paintMaterial, 0);

            _command.SetRenderTarget(support);
            _command.Blit(mask, support);

            _command.SetRenderTarget(extend);
            _command.Blit(mask, extend);

            Graphics.ExecuteCommandBuffer(_command);
            _command.Clear();
        }

        public bool Paintable()
        {
            return _paintable;
        }

        public void Painting(bool p)
        {
            _paintable = p;
        }
    }
}
