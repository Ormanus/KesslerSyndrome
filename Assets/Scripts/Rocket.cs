using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Vector3 dest;
    public int turns;
    public Vector2 destVelocity;
    public Player player;
    Vector3 velocity;

    private Rigidbody2D rb_;
    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleTurn()
    {
        turns--;
        if (turns == 0)
        {
            GetComponent<Collider2D>().isTrigger = false;
            rb_.velocity = (dest - transform.position) / 4.0f;
        }
    }
}
