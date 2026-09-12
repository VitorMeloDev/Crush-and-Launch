using Unity.Microsoft.GDK;
using UnityEngine;

public class ExplosiveBird : MonoBehaviour
{
    private bool activeExplosiveBird;
    [SerializeField] private Drag dragObject;
    private GameInput inputActions;
    [SerializeField] private GameObject bombObj;

    private void Awake()
    {
        inputActions = new GameInput();
    }

    void OnEnable()
    {
        dragObject.OnDragEnd += OnDragBirdEnd;
        inputActions.Enable();
    }

    void OnDragBirdEnd()
    {
        activeExplosiveBird = true;
    }

    void Update()
    {
        if (inputActions.Gameplay.Press.WasPressedThisFrame() && activeExplosiveBird)
        {
            activeExplosiveBird = false;
            Instantiate(bombObj, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
