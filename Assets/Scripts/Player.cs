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
