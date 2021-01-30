using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string name_;
    private Color playerColor_;
    private int money_;
    private List<GameObject> satellites_;

    public string Name
    {
        get { return name_; }
        set { name_ = value; }
    }
    public Color PlayerColor
    {
        get { return playerColor_; }
        set { playerColor_ = value; }
    }
    public int Money
    {
        get { return money_; }
        set { money_ = value; }
    }

    const int START_MONEY = 420;

    public void AddSatellite(GameObject gameObject)
    {
        satellites_.Add(gameObject);
    }

    public void RemovveSatellite(GameObject gameObject)
    {
        satellites_.Remove(gameObject);
    }

    public int SatellitesCount()
    {
        return satellites_.Count;
    }

    public Player(string name, Color playerColor)
    {
        name = Name;
        PlayerColor = playerColor;
        Money = START_MONEY;
        satellites_ = new List<GameObject>();
    }
}
