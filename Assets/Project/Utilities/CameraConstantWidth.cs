using UnityEngine;

namespace Project.Utilities
{
    public class CameraConstantWidth : MonoBehaviour
    {
        public float minSize = 0;
        public Vector2 defaultResolution = new (1920, 1080);
        [Range(0f, 1f)] public float widthOrHeight = .5f;

        private Camera componentCamera;
    
        private float initialSize;
        private float targetAspect;

        private float initialFov;
        private float horizontalFov = 120f;

        private void Start()
        {
            componentCamera = GetComponent<Camera>();
            initialSize = componentCamera.orthographicSize;

            targetAspect = defaultResolution.x / defaultResolution.y;

            initialFov = componentCamera.fieldOfView;
            horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
        }

        private void FixedUpdate()
        {
            if (componentCamera.orthographic)
            {
                var constantWidthSize = initialSize * (targetAspect / componentCamera.aspect);
                var size = Mathf.Lerp(constantWidthSize, initialSize, widthOrHeight);
                componentCamera.orthographicSize = size < minSize ? minSize : size;
            }
            else
            {
                var constantWidthFov = CalcVerticalFov(horizontalFov, componentCamera.aspect);
                componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, widthOrHeight);
            }
        }

        private static float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            var hFovInRads = hFovInDeg * Mathf.Deg2Rad;
            var vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}