using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject finishedLevel;
    [SerializeField] private GameObject lostMenu;

    [Header("Buttons")]
    [SerializeField] private GameObject nextLevelButton;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (PlayerPrefs.GetInt("GameState", 0) != 0) {
            GameManager.Instance.ChangeState((GameState)PlayerPrefs.GetInt("GameState"));
        }

        if (GameManager.Instance.gameState == GameState.Menu) {
            ShowMenu();
            HideLevelMenu();
        }

        if (GameManager.Instance.gameState == GameState.Play) {
            HidePauseMenu();
            HideFinishedLevel();
            HideLostMenu();
        }
    }

    public void ShowMenu() {
        menu.SetActive(true);
    }

    public void HideMenu() {
        menu.SetActive(false);
    }

    public void ShowLevelMenu() {
        levelMenu.SetActive(true);
    }

    public void HideLevelMenu() {
        levelMenu.SetActive(false);
    }

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu() {
        pauseMenu.SetActive(false);
    }

    public void ShowFinishedLevel() {
        finishedLevel.SetActive(true);
    }

    public void HideFinishedLevel() {
        finishedLevel.SetActive(false);
    }

    public void ShowLostMenu() {
        lostMenu.SetActive(true); ;
    }

    public void HideLostMenu() {
        lostMenu.SetActive(false);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(LevelManager.Instance.CurrentLevel.ToString());
        GameManager.Instance.ChangeState(GameState.Play);
    }

    public void NextLevel() {
        int nextLevel = LevelManager.Instance.CurrentLevel + 1;
        SceneManager.LoadScene(nextLevel.ToString());
        GameManager.Instance.ChangeState(GameState.Play);
        LevelManager.Instance.SetCurrentLevel(nextLevel);
    }

    public void MainMenu() {
        SceneManager.LoadScene("Menu");
        PlayerPrefs.SetInt("GameState", (int)GameState.Menu);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
