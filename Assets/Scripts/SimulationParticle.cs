using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationParticle : MonoBehaviour
{
    public float Alpha
    {
        set
        {
            var renderer = GetComponent<SpriteRenderer>();
            Color c = renderer.color;
            renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(value));
        }
    }
}
