using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotBase : MonoBehaviour {
    [SerializeField] private Color color;

    [SerializeField] private GameObject[] walls;

    [SerializeField] private bool ballCollided;
    [SerializeField] private float timeToCount;
    [SerializeField] private float timeAux;
    [SerializeField] private bool firstTime = true;
    public bool ballInsidePot = false;

    [SerializeField] private GameObject ball;

    private void Awake() {
        foreach (GameObject wall in walls) {
            wall.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void Update() {
        if (ballCollided) {
            if (timeAux >= timeToCount) {
                if (firstTime) {
                    if (ball.GetComponent<BallBase>().CompareColor(color)) {
                        ballInsidePot = true;
                        LevelManager.Instance.CheckGameFinished();
                    }
                    firstTime = false;
                }
                return;
            }

            timeAux += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            ball = collision.gameObject;
            ballCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            ballCollided = false;
            ballInsidePot = false;
            firstTime = true;
            timeAux = 0f;
        }
    }
}
