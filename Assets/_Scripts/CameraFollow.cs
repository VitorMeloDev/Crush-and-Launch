using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target, limitLeft, limitRight;
    private Vector3 posCamera;

    // Update is called once per frame
    void Update()
    {
        posCamera = transform.position;
        posCamera.x = target.position.x;
        posCamera.x = Mathf.Clamp(posCamera.x, limitLeft.position.x, limitRight.position.x);
        transform.position = posCamera;
    }
}
