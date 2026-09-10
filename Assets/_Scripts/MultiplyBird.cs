using System;
using UnityEngine;

public class MultiplyBird : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public bool activateMultiplyBird = false;
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private Drag dragObject;
    private GameInput inputActions;
    private GameObject bird_one;
    private GameObject bird_two;

    void Awake()
    {
        inputActions = new GameInput();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        dragObject.OnDragEnd = OnDragBirdEnd;
        inputActions.Enable();
    }

    void OnDragBirdEnd()
    {
        activateMultiplyBird = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Gameplay.Press.WasPressedThisFrame() && activateMultiplyBird)
        {
            activateMultiplyBird = false;
            print("SALVE");
            Vector3 pos = transform.position;
            bird_one = Instantiate(birdPrefab, new Vector3(pos.x, pos.y + 1f), Quaternion.identity);
            bird_two = Instantiate(birdPrefab, new Vector3(pos.x, pos.y - 1f), Quaternion.identity);

            bird_one.GetComponent<Rigidbody2D>().linearVelocity = rigidbody.linearVelocity * 1.05f;
            bird_two.GetComponent<Rigidbody2D>().linearVelocity = rigidbody.linearVelocity * 1.09f;
        }
    }
}
