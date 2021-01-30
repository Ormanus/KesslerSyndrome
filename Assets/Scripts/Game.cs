using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject SatellitePrefab;
    public GameObject CityPrefab;
    public Canvas ScoreCanvas;
    public Text InfoText;
    public Button ButtonNext;
    public Text[] moneyDisplays;

    private List<Player> players_;
    private int currentPlayer_;
    private int satelliteTimer;
    const int SATELLITE_TIMER_LENGTH = 200; //4 seconds;
    const float EARTH_RADIUS = 4.9f;

    private static bool paused_;
    public static bool Paused
    {
        get { return paused_; }
        set { paused_ = value; }
    }

    private void CreateSatellite(Vector3 position, Vector2 velocity, Player player)
    {
        GameObject satellite = Instantiate(SatellitePrefab, position, Quaternion.identity);
        Rigidbody2D rb = satellite.GetComponent<Rigidbody2D>();
        satellite.GetComponent<Physics>().Velocity = velocity;
        satellite.GetComponentInChildren<SpriteRenderer>().color = player.PlayerColor;
        player.AddSatellite(satellite);
    }

    private void CreateCity(Vector2 pos, int pop)
    {
        var obj = Instantiate(CityPrefab);
        obj.transform.position = pos;
        obj.GetComponent<City>().Population = pop;
    }

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        satelliteTimer = 0;
        currentPlayer_ = -1;
        players_ = new List<Player>();
        players_.Add(new Player("Ice Wallowcome", Color.red, moneyDisplays[0]));
        players_.Add(new Player("Sprinkler 777777777777", Color.green, moneyDisplays[1]));
        players_.Add(new Player("Fudge Eynah", Color.blue, moneyDisplays[2]));
        CreateSatellite(new Vector3(7.0f, 3.0f, 0.0f), new Vector2(0.0f, 5.0f), players_[0]);
        CreateSatellite(new Vector3(10.0f, 3.0f, 0.0f), new Vector2(1.0f, 5.0f), players_[1]);

        for (int i = 0; i < 20; i++)
        {
            CreateSatellite(new Vector3(6.0f + i, 1.0f, 0.0f), new Vector2(0.0f, 5.0f - (i / 7f)) + UnityEngine.Random.insideUnitCircle * 0.5f, players_[2]);
        }

        Transform earth = GameObject.Find("Earth")?.transform;
        if (earth == null)
            Debug.LogError("Earth not found!");

        for (int i = 0; i < 10; i++)
        {
            float r = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            Vector3 pos = new Vector3(Mathf.Cos(r), Mathf.Sin(r));

            CreateCity(earth.position + pos * EARTH_RADIUS, UnityEngine.Random.Range(1000, 50000));
        }

        ButtonNext.onClick.AddListener(TaskNext);
        Pause();
    }

    private void TaskNext()
    {
        paused_ = false;
    }

    private void Pause()
    {
        Paused = true;
        satelliteTimer = 0;
        currentPlayer_++;
        if (currentPlayer_ >= players_.Count)
        {
            currentPlayer_ -= players_.Count;
        }

        InfoText.text = players_[currentPlayer_].Name + "'s turn!";
    }

    void Unpause()
    {
        InfoText.text = "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Paused)
        {
            
        }
        else
        {
            if (satelliteTimer >= SATELLITE_TIMER_LENGTH)
            {
                Pause();
                return;
            }
            foreach (Player player in players_)
            {
                player.MonetizeSatellites();
            }
            satelliteTimer++;
        }
    }
}
