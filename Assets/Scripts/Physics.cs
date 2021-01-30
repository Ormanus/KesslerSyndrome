using System;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public Vector2 Velocity;
    public Transform SimulationParticle;

    private GameObject earth_;
    private Rigidbody2D rb_;
    public bool Placed { get; set; } = true;
    const double GM = 4.0; // Gravity constant * mass of earth
    Transform[] simulationParticles = null;

    bool drawParticles = false;

    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        rb_.velocity = Velocity;
        earth_ = GameObject.Find("Earth");

        drawParticles = GetComponent<Satellite>() != null;
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
            rb_.velocity = Vector2.zero;
            if (!Placed)
            {
                Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.0f);

                RecalculateParticles();
            }
            if (simulationParticles == null)
            {
                RecalculateParticles();
            }
        }
        else
        {
            RemoveParticles();
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
                    continue;
                }
                Destroy(simulationParticles[i].gameObject);
            }

            simulationParticles = null;
        }
    }

    public void RecalculateParticles()
    {
        if (!drawParticles)
            return;

        RemoveParticles();

        simulationParticles = new Transform[50];
        rb_.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 simulatedPosition = transform.position;
        Vector2 simulatedSpeed = Velocity;
        int index = 0;
        for (int i = 0; i < 500; i++)
        {
            simulatedPosition += new Vector3(simulatedSpeed.x, simulatedSpeed.y, 0.0f) * 0.02f;
            simulatedSpeed += CalculateMovement(simulatedPosition);
            if (i % 10 == 0)
            {
                var obj = Instantiate(SimulationParticle, simulatedPosition, Quaternion.identity);
                obj.GetComponent<SimulationParticle>().Alpha = 1f - (i / 500f);
                simulationParticles[index++] = obj;
            }
        }
    }

    private void OnDestroy()
    {
        RemoveParticles();
    }
}
