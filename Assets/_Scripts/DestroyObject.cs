using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float time = 1;
    
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
