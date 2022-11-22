using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameState gameState;
    private InputMaster controls;

    [SerializeField] private bool isGamePaused = false;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        PlayerPrefs.SetInt("GameState", 0);
        controls = new InputMaster();
        ChangeState(GameState.Menu);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        controls.Ball.Pause.performed += _ => TogglePause();
    }

    public void ChangeState(GameState newState) {
        gameState = newState;

        switch (gameState) {
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.Lost:
                HandleLost();
                break;
            case GameState.FinishedLevel:
                HandleFinishedLevel();
                break;
            default:
                break;
        }
    }

    private void HandleFinishedLevel() {
        MenuManager.Instance.ShowFinishedLevel();
    }

    private void HandleLost() {
        MenuManager.Instance.ShowLostMenu();
    }

    private void HandlePause() {
        Time.timeScale = 0;
        MenuManager.Instance.ShowPauseMenu();
    }

    private void HandlePlay() {
        Time.timeScale = 1;
        MenuManager.Instance.HidePauseMenu();
        MenuManager.Instance.HideFinishedLevel();
        MenuManager.Instance.HideLostMenu();
    }

    private void HandleMenu() {
        isGamePaused = false;
        MenuManager.Instance.ShowMenu();
        MenuManager.Instance.HideLevelMenu();
    }

    public void TogglePause() {
        if (gameState != GameState.Play && gameState != GameState.Pause)
            return;

        GameState state = isGamePaused ? GameState.Play : GameState.Pause;
        isGamePaused = !isGamePaused;
        ChangeState(state);
    }

    private void OnEnable() {
        if (controls != null)
            controls.Enable();
    }
    private void OnDisable() {
        if (controls != null)
            controls.Disable();
    }
}

public enum GameState {
    Menu = 1,
    Play = 2,
    Pause = 3,
    Lost = 4,
    FinishedLevel = 5,
}
