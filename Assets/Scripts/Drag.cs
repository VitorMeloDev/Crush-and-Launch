using System;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D collider;
    public LayerMask layerMask;
    [SerializeField] private bool isDragging = false;
    private Touch touch;

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    private Ray leftCatapultRay;
    private CircleCollider2D circleCollider;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D springJoint;
    private Vector2 prevVel;
    private Rigidbody2D rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();  
        circleCollider = GetComponent<CircleCollider2D>();
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        springJoint = GetComponent<SpringJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);

        // Update the line renderer positions in the Update method
        lineFront.SetPosition(1, transform.position);
        lineBack.SetPosition(1, transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        // Update the line renderer positions in the Update method
        catapultToBird = transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;

        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + circleCollider.radius);

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL);

        SpringEffect();
        prevVel = rigidbody.linearVelocity;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            Vector2 wp = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, layerMask);

            if(hit.collider != null)
            {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(touch.position);
                    transform.position = tPos;
                    isDragging = true;

                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }
                Debug.Log("Touching " + hit.collider.name);
            }
        }
    }

    void SpringEffect()
    {
        if (springJoint != null)
        {
            if (rigidbody.bodyType != RigidbodyType2D.Kinematic)
            {
                if (prevVel.sqrMagnitude > rigidbody.linearVelocity.sqrMagnitude)
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
