using System;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D collider;
    public LayerMask layerMask;
    [SerializeField] private bool isDragging = false;
    private GameInput inputActions;

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    private Ray leftCatapultRay;
    private CircleCollider2D circleCollider;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D springJoint;
    private Vector2 prevVel;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        inputActions = new GameInput();
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        springJoint = GetComponent<SpringJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);

        lineFront.SetPosition(1, transform.position);
        lineBack.SetPosition(1, transform.position);
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
        catapultToBird = transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;

        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + circleCollider.radius);

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL);

        SpringEffect();
        prevVel = rigidbody.linearVelocity;

        Vector2 positionInput = inputActions.Gameplay.Point.ReadValue<Vector2>();

        Vector2 wp = Camera.main.ScreenToWorldPoint(positionInput);

        if (inputActions.Gameplay.Press.WasPressedThisFrame())
        {
            Collider2D hit = Physics2D.OverlapPoint(wp, layerMask);

            if (hit != null)
            {
                isDragging = true;
            }
        }

        if (isDragging &&
            inputActions.Gameplay.Press.IsPressed())
        {
            transform.position = wp;
        }

        if (inputActions.Gameplay.Press.WasReleasedThisFrame())
        {
            isDragging = false;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void SpringEffect()
    {
        if (springJoint != null)
        {
            if (rigidbody.bodyType != RigidbodyType2D.Kinematic)
            {
                if (prevVel.sqrMagnitude >
                    rigidbody.linearVelocity.sqrMagnitude)
                {
                    lineFront.enabled = false;
                    lineBack.enabled = false;

                    Destroy(springJoint);

                    rigidbody.linearVelocity = prevVel;
                }
            }
        }
    }
}