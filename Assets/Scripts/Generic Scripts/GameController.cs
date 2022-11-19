using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MenuUI
{
    public static GameController Instance;
    [SerializeField] private WaveController waveController;
    [SerializeField] private PlayerController playerController;
    [FormerlySerializedAs ("uiGroup")] public CanvasGroup MenuCanvas;
    public Button exitGameButton;
    public float turretSmoothFactor;
    public enum ShootStyle {
        OBJECT_POOLING,
        RAYCAST
    }

    public ShootStyle CurrentShootStyle;

    public enum GameState {
        MENU,
        RUNNING
    }
    private GameState currentGameState = GameState.MENU;
    [SerializeField] private Animator LogoAnimator;
    
    public Action HideAllWeaponUI;
    public Action ExitGamePlay;
    public Action StartGamePlay;
    private void Awake () {
        if(Instance == null) Instance = this;
    }

    private void Start() {
        LogoAnimator.enabled = true;
    }
    // Start is called before the first frame update
    public void StartGame() {
        StartCoroutine(HideMenu());
    }

    public bool IsGameRunning() {
        return currentGameState == GameState.RUNNING;
    }

    IEnumerator HideMenu() {
        float temp = MenuCanvas.alpha;
        while(temp > 0) {
            temp -= Time.deltaTime;
            MenuCanvas.alpha = temp;
            yield return new WaitForEndOfFrame();
        }
        StartGamePlay ();
        LogoAnimator.enabled = false;
        MenuCanvas.alpha = 0;
        MenuCanvas.blocksRaycasts = false;
        exitGameButton.interactable = true;
        exitGameButton.gameObject.SetActive(true);
        currentGameState = GameState.RUNNING;
    }

    IEnumerator ShowMenu() {
        exitGameButton.interactable = false;
        exitGameButton.gameObject.SetActive(false);
        float temp = MenuCanvas.alpha;
        LogoAnimator.enabled = true;
        while (temp < 1) {
            yield return new WaitForEndOfFrame();
            temp += Time.deltaTime;
            MenuCanvas.alpha = temp;
        }
        MenuCanvas.alpha = 1;
        MenuCanvas.blocksRaycasts = true;
        currentGameState = GameState.MENU;
    }

    public void ExitGame() {
        HideAllWeaponUI ();
        if (ExitGamePlay is not null) ExitGamePlay ();
        waveController.ResetWave();
        waveController.ResetGame();
        playerController.ResetTurrets();
        StartCoroutine(ShowMenu());
    }

    public void QuitGame() {
        Application.Quit();
    }
}
