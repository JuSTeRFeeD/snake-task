using ME.ECS;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{
    [SerializeField] private GlobalEvent startGame;
    [SerializeField] private GlobalEvent endGame;
    [Space]
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private Canvas playingCanvas;
    [SerializeField] private Canvas endGameCanvas;
    [SerializeField] private Button restartButton;

    private PanelsState panelsState;

    private enum PanelsState
    {
        Loading,
        Gameplay,
        End
    }

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
            
        startGame.Subscribe(OnStartGame);
        endGame.Subscribe(OnEndGame);
            
        SetState(PanelsState.Loading);
    }

    private void RestartGame()
    {
        restartButton.interactable = false;
        restartButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Restarting");
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        startGame.Unsubscribe(OnStartGame);
        endGame.Unsubscribe(OnEndGame);
    }

    private void OnEndGame(in Entity entity) => SetState(PanelsState.End);

    private void OnStartGame(in Entity entity) => SetState(PanelsState.Gameplay);

    private void SetState(PanelsState state)
    {
        panelsState = state;
            
        loadingCanvas.gameObject.SetActive(panelsState == PanelsState.Loading);
        playingCanvas.gameObject.SetActive(panelsState == PanelsState.Gameplay);
        endGameCanvas.gameObject.SetActive(panelsState == PanelsState.End);
    }
}