using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private WaveController waveController;
    private PlayerGamePlayController playerController;
    public CanvasGroup uiGroup;
    public Button exitGameButton;
    public float turretSmoothFactor;

    public enum GameState {
        MENU,
        RUNNING
    }
    private GameState currentGameState = GameState.MENU;
    

    private void Start() {
        if(Instance == null) {
            Instance = this;
        }
        playerController = FindObjectOfType<PlayerGamePlayController>();
        waveController = FindObjectOfType<WaveController>();
    }
    // Start is called before the first frame update
    public void StartGame() {
        StartCoroutine(HideMenu());
    }

    public bool IsGameRunning() {
        return currentGameState == GameState.RUNNING;
    }

    IEnumerator HideMenu() {
        float temp = uiGroup.alpha;
        while(temp > 0) {
            yield return new WaitForEndOfFrame();
            temp -= Time.deltaTime;
            uiGroup.alpha = temp;
        }

        uiGroup.alpha = 0;
        exitGameButton.interactable = true;
        exitGameButton.gameObject.SetActive(true);
        waveController.StartGame();
        currentGameState = GameState.RUNNING;
    }

    IEnumerator ShowMenu() {
        exitGameButton.interactable = false;
        exitGameButton.gameObject.SetActive(false);
        float temp = uiGroup.alpha;
        while (temp < 1) {
            yield return new WaitForEndOfFrame();
            temp += Time.deltaTime;
            uiGroup.alpha = temp;
        }
        uiGroup.alpha = 1;
        currentGameState = GameState.MENU;
    }

    public void ExitGame() {
        waveController.ResetWave();
        playerController.ResetTurrets();
        playerController.ToggleScoreCard(false);
        playerController.ToggleGemObject(false);
        StartCoroutine(ShowMenu());
    }

    public void QuitGame() {
        Application.Quit();
    }
}
