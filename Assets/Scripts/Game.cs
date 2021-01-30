using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject SatellitePrefab;
    public Canvas ScoreCanvas;
    public Text InfoText;
    public Button ButtonNext;

    private List<Player> players_;
    private int currentPlayer_;
    private int satelliteTimer;
    const int SATELLITE_TIMER_LENGTH = 200; //4 seconds;

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

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        satelliteTimer = 0;
        currentPlayer_ = -1;
        players_ = new List<Player>();
        players_.Add(new Player("Ice Wallowcome", Color.red));
        players_.Add(new Player("Sprinkler 777777777777", Color.green));
        players_.Add(new Player("Fudge Eynah", Color.blue));
        CreateSatellite(new Vector3(7.0f, 1.0f, 0.0f), new Vector2(0.0f, -5.0f), players_[0]);
        CreateSatellite(new Vector3(8.0f, -2.0f, 0.0f), new Vector2(1.0f, 5.0f), players_[1]);
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
