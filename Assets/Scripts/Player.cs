using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    private List<GameObject> satellites_;
    private Transform earth;
    private int money_;

    public Text moneyDisplay;

    public string Name { get; set; }
    public Color PlayerColor { get; set; }
    public int Money
    {
        get { return money_; }
        set {
            money_ = value;
            if (moneyDisplay != null)
                moneyDisplay.text = Name + ": " + Money;
        }
    }


    public int SatellitesCount()
    {
        return satellites_.Count;
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
                    Money += city.Population;
                }
            }
        }
    }

    public void AddSatellite(GameObject gameObject)
    {
        gameObject.GetComponent<Satellite>().player = this;
        satellites_.Add(gameObject);
    }

    public void RemoveSatellite(GameObject gameObject)
    {
        satellites_.Remove(gameObject);
    }

    public GameObject ClosestSatellite(Vector2 mousePos)
    {
        if (satellites_.Count == 0)
        {
            return null;
        }
        Vector3 pos = satellites_[0].transform.position;
        float dx = mousePos.x - pos.x;
        float dy = mousePos.y - pos.y;
        float closestDist = dx * dx + dy * dy;
        GameObject closestSatellite = satellites_[0];
        for (int i = 1; i < satellites_.Count; i++)
        {
            pos = satellites_[i].transform.position; 
            dx = mousePos.x - pos.x;
            dy = mousePos.y - pos.y;
            float dist = dx * dx + dy * dy;
            if (dist < closestDist)
            { 
                closestDist = dist;
                closestSatellite = satellites_[i];
            }
        }
        return closestSatellite;
    }

    public Player(string name, Color playerColor, Text display)
    {
        Name = name;
        PlayerColor = playerColor;
        Money = START_MONEY;
        satellites_ = new List<GameObject>();
        earth = GameObject.Find("Earth").transform;
        moneyDisplay = display;
    }
}
