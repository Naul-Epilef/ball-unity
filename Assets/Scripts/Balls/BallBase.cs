using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallBase : MonoBehaviour {
    [SerializeField] private InputMaster controls;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;

    [SerializeField] private Color color;
    [SerializeField] private float force;
    [SerializeField] private bool aiming = false;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float maxDistance;

    [SerializeField] private float _maxSpeedToAim;

    [Space(5)]
    [Header("Boundaries")]
    [SerializeField] private Vector2 screenBounds;
    [SerializeField] private float ballWidth;
    [SerializeField] private float ballHeight;

    private void Awake() {
        controls = new InputMaster();
        GetComponent<SpriteRenderer>().color = color;
    }

    private void Start() {
        lr.startColor = color;
        lr.endColor = color;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        ballWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        ballHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        controls.Ball.AimFire.started += _ =>
        {
            if (GameManager.Instance.gameState != GameState.Play)
                return;

            aiming = true;
        };
        controls.Ball.AimFire.canceled += _ =>
        {
            if (GameManager.Instance.gameState != GameState.Play)
                return;

            Fire();
            aiming = false;
        };
    }

    private void Update() {
        if (GameManager.Instance.gameState != GameState.Play)
            return;

        if (aiming && rb.velocity.magnitude <= _maxSpeedToAim) {
            Aim();
        } else {
            lr.positionCount = 0;
        }
    }

    private void LateUpdate() {
        Vector2 viewPos = transform.position;

        // viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 - ballWidth, screenBounds.x + ballWidth);
        // viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 - ballHeight, screenBounds.y + ballHeight);

        if ((viewPos.x < screenBounds.x * -1 - ballWidth  || viewPos.x > screenBounds.x + ballWidth) ||
            (viewPos.y < screenBounds.y * -1 - ballHeight || viewPos.y > screenBounds.y + ballHeight)) {
            Kill();
            return;
        }
    }

    public bool CompareColor(Color potColor) {
        return potColor == color;
    }

    private void Aim() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position.ConvertTo<Vector2>();
        direction = Vector2.ClampMagnitude(direction, maxDistance);
        Plot(transform.position, force * direction, 500);
    }

    private void Fire() {
        if (!aiming || rb.velocity.magnitude > _maxSpeedToAim)
            return;

        // rb.AddForce(force * distance * direction, ForceMode2D.Force);
        rb.velocity = force * direction;

        lr.positionCount = 0;
        direction = Vector2.zero;
    }

    private void Plot(Vector2 pos, Vector2 velocity, int steps) {
        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = rb.gravityScale * Mathf.Pow(timestep, 2) * Physics2D.gravity;

        float drag = 1f - timestep * rb.drag;
        Vector2 moveStep = velocity * timestep;

        lr.positionCount = steps;

        for (int i = 0; i < steps; i++) {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            lr.SetPosition(i, pos);
        }
    }

    private void Kill() {
        GameManager.Instance.ChangeState(GameState.Lost);
        Destroy(gameObject);
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
