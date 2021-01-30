using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private List<GameObject> satellites_;
    private Transform earth;

    public string Name { get; set; }
    public Color PlayerColor { get; set; }
    public int Money { get; set; }

    public int SatellitesCount
    {
        get { return satellites_.Count; }
    }

    const int START_MONEY = 420;

    public void MonetizeSatellites()
    {
        foreach (var satellite in satellites_)
        {
            Vector2 delta0 = satellite.transform.position - earth.position;
            float angle0 = Mathf.Atan2(delta0.y, delta0.x);

            foreach (City city in City.AllCities)
            {
                if (Mathf.Abs(city.Angle - angle0) < 0.1f)
                {
                    Money++;
                }
            }
        }
    }

    public void AddSatellite(GameObject gameObject)
    {
        satellites_.Add(gameObject);
    }

    public void RemovveSatellite(GameObject gameObject)
    {
        satellites_.Remove(gameObject);
    }

    public Player(string name, Color playerColor)
    {
        name = Name;
        PlayerColor = playerColor;
        Money = START_MONEY;
        satellites_ = new List<GameObject>();
        earth = GameObject.Find("Earth").transform;
    }
}
