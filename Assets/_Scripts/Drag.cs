using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drag : MonoBehaviour
{
    [Header("Bird")]
    public GameObject birdDeathEffect;
    [Header("Drag")]
    public LayerMask layerMask;
    [SerializeField] private bool isDragging = false;
    private Collider2D collider;
    private GameInput inputActions;
    private Camera mainCamera;
    public Action OnDragEnd;

    [Header("Spring Joint")]
    private SpringJoint2D springJoint;
    private Vector2 prevVel;
    private Rigidbody2D rigidbody;

    private Transform catapult;
    private Ray rayToMT;
    private Vector2 catapultToBird;

    private void Awake()
    {
        inputActions = new GameInput();
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        springJoint = GetComponent<SpringJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        catapult = springJoint.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        SpringEffect();
        Dragging();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }

    void Dragging()
    {
        prevVel = rigidbody.linearVelocity;

        Vector2 positionInput = inputActions.Gameplay.Point.ReadValue<Vector2>();

        Vector2 wp = mainCamera.ScreenToWorldPoint(positionInput);

        if (inputActions.Gameplay.Press.WasPressedThisFrame())
        {
            Collider2D hit = Physics2D.OverlapPoint(wp, layerMask);
            Debug.Log("Hit: " + hit);
            if (hit != null)
            {
                isDragging = true;
            }
        }

        if (isDragging && inputActions.Gameplay.Press.IsPressed())
        {
            catapultToBird = wp - (Vector2)catapult.position;

            if (catapultToBird.sqrMagnitude > 9f)
            {
                rayToMT.direction = catapultToBird;
                wp = rayToMT.GetPoint(3f);
            }

            transform.position = wp;
        }

        if (isDragging && inputActions.Gameplay.Press.WasReleasedThisFrame())
        {
            isDragging = false;
            OnStopMove();
            OnDragEnd?.Invoke();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void SpringEffect()
    {
        if (springJoint == null)
            return;
        if (rigidbody.bodyType != RigidbodyType2D.Kinematic)
        {
            if (prevVel.sqrMagnitude >
                rigidbody.linearVelocity.sqrMagnitude)
            {
                Destroy(springJoint);

                rigidbody.linearVelocity = prevVel;
            }
        }
    }

    public bool IsDragging()
    {
        return isDragging;
    }

    void OnStopMove()
    {
        if (rigidbody.linearVelocity.magnitude < 0f && rigidbody.IsSleeping())
        {
            StartCoroutine(StopMove());
        }
    }

    IEnumerator StopMove()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(birdDeathEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}