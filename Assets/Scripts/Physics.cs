using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public Vector2 Velocity;
    public Transform SimulationParticle;
    private GameObject earth;
    private Rigidbody2D rb;
    private bool started = false;
    const double GM = 1.2; // Gravity constant * mass of earth
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Velocity;
        earth = GameObject.Find("Earth");
        started = true;
    }

    private Vector2 CalculateMovement(Vector3 pos)
    {
        double dx = earth.transform.position.x - pos.x;
        double dy = earth.transform.position.y - pos.y;
        double r = Math.Sqrt(dx * dx + dy * dy);
        double acceleration = GM / (r * r);
        double angle = Math.Atan2(dy, dx);
        return new Vector2((float)(acceleration * Math.Cos(angle)), (float)(acceleration * Math.Sin(angle)));
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (!started)
        {
            return;
        }
        rb.velocity += CalculateMovement(transform.position);
        Vector3 simulatedPosition = transform.position;
        Vector2 simulatedSpeed = rb.velocity;
        for (int i = 0; i < 100; i++)
        {
            simulatedPosition += new Vector3(simulatedSpeed.x, simulatedSpeed.y, 0.0f) * 0.02f;
            simulatedSpeed += CalculateMovement(simulatedPosition);
            if (i % 10 == 0)
            {
                Instantiate(SimulationParticle, simulatedPosition, Quaternion.identity);
            }
        }
    }
}
