using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float frameTime = 0.1f;
    public Sprite[] sprites;

    private float startTime = 0f;
    private SpriteRenderer sr;


    private void Awake()
    {
        startTime = Time.time;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
    }

    private void Update()
    {
        int spriteNumber = (int)((Time.time - startTime) / frameTime);

        if (spriteNumber >= sprites.Length)
        {
            Destroy(gameObject);
            return;
        }

        sr.sprite = sprites[spriteNumber];
    }
}
