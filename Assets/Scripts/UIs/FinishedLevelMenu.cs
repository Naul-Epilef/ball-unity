using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedLevelMenu : MonoBehaviour {
    [SerializeField] private GameObject nextLevelButton;

    public void CheckNextLevelButton() {
        nextLevelButton.SetActive(!LevelManager.Instance.isLastLevel());
    }

    private void OnEnable() {
        CheckNextLevelButton();
    }
}
