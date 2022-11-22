using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour {
    [SerializeField] private GameObject content;

    [SerializeField] private GameObject levelButton;

    private void Awake() {
        bool isSearchingLevels = true;
        int i = 1;
        while (isSearchingLevels) {
            if (LevelManager.Instance.DoesSceneExist(i.ToString())) {
                GameObject newButton = Instantiate(levelButton);

                int sceneName = i;
                string name = $"Level {i}";

                newButton.GetComponentInChildren<TextMeshProUGUI>().text = name;
                newButton.GetComponent<Button>().onClick.AddListener(
                    delegate {
                        LevelManager.Instance.SetCurrentLevel(sceneName);
                        SceneManager.LoadScene(sceneName);
                        PlayerPrefs.SetInt("GameState", (int)GameState.Play);
                    });

                newButton.transform.SetParent(content.transform, false);
                i++;
            } else {
                isSearchingLevels = false;
            }
        }
    }
}
