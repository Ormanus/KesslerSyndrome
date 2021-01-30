using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public static List<City> AllCities = new List<City>();

    public int Population;

    public float Angle;

    private void Awake()
    {
        Transform earth = GameObject.Find("Earth")?.transform;
        if (earth == null)
            Debug.LogError("Earth not found!");

        AllCities.Add(this);

        Vector2 delta = transform.position - earth.position;
        Angle = Mathf.Atan2(delta.y, delta.x);
    }
}
