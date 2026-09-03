using UnityEngine;

public class EffectDragCatapult : MonoBehaviour
{
    [SerializeField] private LineRenderer lineFront;
    [SerializeField] private LineRenderer lineBack;
    [SerializeField] private Drag dragObject;
    [SerializeField] private Vector2 catapultToBird;
    [SerializeField] private Vector3 pointL;
    private Ray leftCatapultRay;
    private CircleCollider2D circleColliderDragObject;

    private void OnEnable()
    {
        dragObject.OnDragEnd += DesactivateLineRenderers;
    }

    private void OnDisable()
    {
        dragObject.OnDragEnd -= DesactivateLineRenderers;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleColliderDragObject = dragObject.GetComponent<CircleCollider2D>();
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);

        SetLineRendererPositions(0, lineFront.transform.position, lineBack.transform.position); 
        SetLineRendererPositions(1, dragObject.transform.position, dragObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        GetToBird();
    }

    void SetLineRendererPositions(int index, Vector3 positionFront, Vector3 positionBack)
    {
        lineFront.SetPosition(index, positionFront);
        lineBack.SetPosition(index, positionBack);
    }

    void GetToBird()
    {
        catapultToBird = dragObject.transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;
        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + circleColliderDragObject.radius);

        SetLineRendererPositions(1, pointL, pointL);
    }

    void DesactivateLineRenderers()
    {
        lineFront.enabled = false;
        lineBack.enabled = false;
    }
}
