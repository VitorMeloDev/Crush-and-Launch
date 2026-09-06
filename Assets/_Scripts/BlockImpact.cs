using UnityEngine;

public class BlockImpact : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int currentImpactIndex = 0; // Index to track the current impact sprite
    [SerializeField] private Sprite[] impactSprites; // Array to hold the impact sprites
    [SerializeField] private GameObject impactEffectPrefab; // Prefab for the impact effect

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void BlockDestroy()
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 4f && collision.relativeVelocity.magnitude < 10f)
        {
            if (currentImpactIndex < impactSprites.Length - 1)
            {
                spriteRenderer.sprite = impactSprites[++currentImpactIndex];
            }
            else
            {
                BlockDestroy();
            }
        }
        else if (collision.relativeVelocity.magnitude >= 10f)
        {
            BlockDestroy();
        }
    }
}
