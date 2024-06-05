using UnityEngine;

namespace _Scripts
{
    public class TargetFrameRate : MonoBehaviour
    {
        private const int DefaultTargetFrame = -1;

        private void Awake()
        {
#if UNITY_ANDROID

            Application.targetFrameRate = int.Parse(Screen.currentResolution.refreshRateRatio.ToString());
#endif
        }
    }
}