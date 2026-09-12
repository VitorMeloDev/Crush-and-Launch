using UnityEngine;

public class TrailOff : MonoBehaviour
{
    [SerializeField] private Transform trail;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject && trail != null)
        {
            trail.parent = null;
            trail = null;
        }
    }
}
