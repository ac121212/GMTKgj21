using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float DestroyAfterSeconds = 1.2f;
    public void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    public IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(DestroyAfterSeconds);
        Destroy(this.gameObject);
    }

    
}
