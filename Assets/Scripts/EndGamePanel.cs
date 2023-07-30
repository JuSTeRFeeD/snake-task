using System.Text;
using ME.ECS;
using Project.Components;
using TMPro;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private GlobalEvent endGameDataReceived;
    [Space] [SerializeField] private GameObject loader;
    [Space] [SerializeField] private GameObject content;
    [SerializeField] private TextMeshProUGUI info;

    private void Start()
    {
        endGameDataReceived.Subscribe(OnReceived);
    }

    private void OnEnable()
    {
        loader.SetActive(true);
        content.SetActive(false);
    }

    private void OnDestroy()
    {
        endGameDataReceived.Unsubscribe(OnReceived);
    }

    private void OnReceived(in Entity entity)
    {
        loader.SetActive(false);
        content.SetActive(true);

        var world = Worlds.currentWorld;
        ref readonly var gameEndInfo = ref world.GetSharedData<GameEndInfo>();

        var sb = new StringBuilder();
        sb.Append($"Collected apples: {gameEndInfo.gameInfo.appleCount}");
        sb.Append("\n");
        sb.Append($"Snake length: {gameEndInfo.gameInfo.snakeLength}");
        sb.Append("\n");
        if (gameEndInfo.playedTime % 60 > 0)
        {
            sb.Append($"Play time: {gameEndInfo.playedTime} sec.");
        }
        info.SetText(sb);
    }
}