using UnityEngine;

public class FasterBird : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private bool activeFasterBird = false;
    [SerializeField] private Drag dragObject;
    private GameInput inputActions;

    private void Awake()
    {
        inputActions = new GameInput();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        dragObject.OnDragEnd = OnDragBirdEnd;
        inputActions.Enable();
    }

    void OnDragBirdEnd()
    {
        activeFasterBird = true;
    }

    void Update()
    {
        if (inputActions.Gameplay.Press.WasPressedThisFrame() && activeFasterBird)
        {
            activeFasterBird = false;
            print("Olá");
            rigidbody.linearVelocity = rigidbody.linearVelocity * 1.5f;
        }
    }
}
