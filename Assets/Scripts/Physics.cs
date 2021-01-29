using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public GameObject Earth;
    private Rigidbody2D rb;
    private bool started = false;
    private Vector2 previousVelocity;
    const double GM = 1.2; // Gravity constant * mass of earth
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-3.0f, 3.0f);
        previousVelocity = rb.velocity;
        started = true;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (!started)
        {
            return;
        }
        double dx = Earth.transform.position.x - transform.position.x;
        double dy = Earth.transform.position.y - transform.position.y;
        double r = Math.Sqrt(dx * dx + dy * dy);
        double acceleration = GM / (r * r);
        double angle = Math.Atan2(dy, dx);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity += new Vector2((float)(acceleration * Math.Cos(angle)), (float)(acceleration * Math.Sin(angle)));
        previousVelocity = rb.velocity;
    }
}
