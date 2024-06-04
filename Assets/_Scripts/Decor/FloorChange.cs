using UnityEngine;

namespace _Scripts.Decor
{
    public class FloorChange : Singleton<FloorChange>
    {
        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        }
    
        public void Change(Material mat)
        {
            gameObject.GetComponent<Renderer>().material = mat;
        }
    }
}
