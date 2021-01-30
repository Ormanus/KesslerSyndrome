using System;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public Vector2 Velocity;
    public Transform SimulationParticle;

    private GameObject earth_;
    private Rigidbody2D rb_;
    private bool placed_ = false;
    public bool Placed
    {
        get { return placed_; }
        set { placed_ = value; }
    }
    const double GM = 4.0; // Gravity constant * mass of earth
    Transform[] simulationParticles = null;

    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        rb_.velocity = Velocity;
        earth_ = GameObject.Find("Earth");
    }

    private Vector2 CalculateMovement(Vector3 pos)
    {
        double dx = earth_.transform.position.x - pos.x;
        double dy = earth_.transform.position.y - pos.y;
        double r = Math.Sqrt(dx * dx + dy * dy);
        double acceleration = GM / (r * r);
        double angle = Math.Atan2(dy, dx);
        return new Vector2((float)(acceleration * Math.Cos(angle)), (float)(acceleration * Math.Sin(angle)));
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (Game.Paused)
        {
            if (!Placed)
            {
                Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.0f);
            }
            if (simulationParticles == null)
            {
                RecalculateParticles();
            }
        }
        else
        {


            rb_.velocity = Velocity + CalculateMovement(transform.position);
            Velocity = rb_.velocity;
        }
    }

    void RemoveParticles()
    {
        if (simulationParticles != null)
        {
            for (int i = 0; i < simulationParticles.Length; i++)
            {
                if (simulationParticles[i] == null)
                {
                    Debug.LogWarning(i);
                    continue;
                }
                Destroy(simulationParticles[i].gameObject);
            }

            simulationParticles = null;
        }
    }

    public void RecalculateParticles()
    {
        RemoveParticles();

        simulationParticles = new Transform[25];
        rb_.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 simulatedPosition = transform.position;
        Vector2 simulatedSpeed = Velocity;
        int index = 0;
        for (int i = 0; i < 500; i++)
        {
            simulatedPosition += new Vector3(simulatedSpeed.x, simulatedSpeed.y, 0.0f) * 0.02f;
            simulatedSpeed += CalculateMovement(simulatedPosition);
            if (i % 20 == 0)
            {
                simulationParticles[index++] = Instantiate(SimulationParticle, simulatedPosition, Quaternion.identity);
            }
        }
    }

    private void OnDestroy()
    {
        RemoveParticles();
    }
}
