using ME.ECS;
using Project.Markers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MobilePlayerInput : MonoBehaviour
{
    [SerializeField] private Button up;
    [SerializeField] private Button down;
    [SerializeField] private Button left;
    [SerializeField] private Button right;

    private void Start()
    {
        up.onClick.AddListener(OnUp);
        down.onClick.AddListener(OnDown);
        left.onClick.AddListener(OnLeft);
        right.onClick.AddListener(OnRight);
    }

    private static void OnUp() => UpdateDirection(new int2(0, 1));
    private static void OnDown() => UpdateDirection(new int2(0, -1));
    private static void OnLeft() => UpdateDirection(new int2(-1, 0));
    private static void OnRight() => UpdateDirection(new int2(1, 0));

    private static void UpdateDirection(int2 dir)
    {
        var world = Worlds.currentWorld;
        world.AddMarker(new PlayerMoveInputMarker() { value = dir });
    }
}
