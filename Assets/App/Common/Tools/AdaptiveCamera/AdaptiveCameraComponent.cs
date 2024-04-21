
using App.Core.Common;
using UnityEngine;

namespace App.Common.Tools.AdaptiveCamera
{
    public class AdaptiveCameraComponent : AppMonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private bool isRunInUpdate;
        [SerializeField] private AdaptiveCameraVerticalBinding binding;
        
        [Header("Camera Settings")]
        [SerializeField] private float defaultWidth;
        [SerializeField] private float defaultOrthographicSize;

        public void ApplyCurrentParameters(Camera camera)
        {
            if (camera == null)
            {
                return;
            }
            
            defaultWidth = camera.orthographicSize * camera.aspect;
            defaultOrthographicSize = camera.orthographicSize;
        }

        private void Update()
        {
            if (isRunInUpdate)
            {
                SetCameraSettings();
            }
        }

        private void SetCameraSettings()
        {
            CachedCamera.orthographicSize = defaultWidth / CachedCamera.aspect;
            var cameraPosition = CachedCamera.transform.position;
            float bias = defaultOrthographicSize - CachedCamera.orthographicSize;

            if (Mathf.Abs(bias) < 0.01f)
            {
                return;
            }

            float yPos = bias * (int) binding;
            CachedCamera.transform.position = new Vector3(cameraPosition.x, yPos, cameraPosition.z);
        }
    }
}