using DG.Tweening;
using ME.ECS;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GlobalEvent foodEaten;
    [SerializeField] private Transform cameraTransform;
    
    private void Start()
    {
        foodEaten.Subscribe(ShakeCamera);
    }

    private void OnDestroy()
    {
        foodEaten.Unsubscribe(ShakeCamera);
    }

    private void ShakeCamera(in Entity entity)
    {
        cameraTransform.DOShakePosition(0.15f, 0.2f, 2).SetLink(gameObject);
    }
}
