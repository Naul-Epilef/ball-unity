using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;

    [SerializeField] private GameObject[] pots;
    [SerializeField] private GameObject[] balls;

    [field: SerializeField] public int CurrentLevel { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Instance.FindAllPots();
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Instance.FindAllPots();
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentLevel(string level) {
        CurrentLevel = int.Parse(level);
    }

    public void SetCurrentLevel(int level) {
        CurrentLevel = level;
    }

    public bool isLastLevel() {
        return !DoesSceneExist((CurrentLevel + 1).ToString());
    }

    public void FindAllPots() {
        pots = GameObject.FindGameObjectsWithTag("Pot");
        balls = GameObject.FindGameObjectsWithTag("Ball");
    }

    public void CheckGameFinished() {
        int amountPotWithBall = 0;
        foreach (GameObject pot in pots) {
            if (pot.GetComponent<PotBase>().ballInsidePot) {
                amountPotWithBall++;
            }
        }

        if (amountPotWithBall == pots.Length) {
            GameManager.Instance.ChangeState(GameState.FinishedLevel);
        }
    }

    public bool DoesSceneExist(string name) {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }
}
