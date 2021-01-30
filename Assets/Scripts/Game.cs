using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject SatellitePrefab;
    private List<Player> players_;

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
        players_ = new List<Player>();
        players_.Add(new Player("Ice Wallow", Color.red));
        players_.Add(new Player("Sprinkler 777777777777", Color.green));
        players_.Add(new Player("Fudge Eynah", Color.blue));
        CreateSatellite(new Vector3(7.0f, 1.0f, 0.0f), new Vector2(0.0f, -3.0f), players_[0]);
        CreateSatellite(new Vector3(8.0f, -2.0f, 0.0f), new Vector2(1.0f, 3.0f), players_[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}