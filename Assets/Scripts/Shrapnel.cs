using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel : MonoBehaviour
{
    public GameObject ExplosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        obj.transform.localScale *= 0.3f;
        Destroy(gameObject);
    }
}
