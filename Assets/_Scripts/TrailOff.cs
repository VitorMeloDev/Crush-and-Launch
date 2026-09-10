using UnityEngine;

public class TrailOff : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
            transform.DetachChildren();
    }
}
