
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public GameObject SatellitePrefab;
    public GameObject CityPrefab;
    public Canvas ScoreCanvas;
    public Text InfoText;
    public Button ButtonNext;
    public Button ButtonCreate;
    public Button ButtonCancelCreate;
    public Button ButtonMove;
    public Button ButtonCancelMove;
    public Button ButtonUpgrade;
    public Text[] moneyDisplays;
    public Text roundDisplay;

    private List<Player> players_;
    private int currentPlayer_;
    private int satelliteTimer;
    private bool placingSatellite_ = false;
    private GameObject handledSatellite_ = null;
    private int roundCount = 24;

    const int SATELLITE_TIMER_LENGTH = 200; //4 seconds;
    const int FINAL_ROUND_LENGTH = 600; //12 seconds?
    const float EARTH_RADIUS = 4.9f;

    public static Game Instance;

    private static bool paused_;
    public static bool Paused
    {
        get { return paused_; }
        set { paused_ = value; }
    }
    public static Player CurrentPlayer
    {
        get
        {
            if (Instance == null || Instance.currentPlayer_ == -1)
                return null;
            return Instance.players_[Instance.currentPlayer_];
        }
    }

    public enum ActionType
    {
        None,
        CreateSatellite,
        MoveSatellite
    }
    public ActionType currentAction = ActionType.None;

    List<GameObject> rockets_ = new List<GameObject>();

    private GameObject CreateSatellite(Vector3 position, Vector2 velocity, Player player)
    {
        GameObject satellite = Instantiate(SatellitePrefab, position, Quaternion.identity);
        Rigidbody2D rb = satellite.GetComponent<Rigidbody2D>();
        satellite.GetComponent<Physics>().Velocity = velocity;
        satellite.GetComponentInChildren<SpriteRenderer>().color = player.PlayerColor;
        satellite.GetComponent<Physics>().Placed = true;
        var sat = satellite.GetComponent<Satellite>();
        sat.thrust = player.thrust * 0.5f;
        sat.comms = player.comms;
        sat.armor = player.armor;
        player.AddSatellite(satellite);
        return satellite;
    }

    private void CreateCity(Vector2 pos, int pop)
    {
        var obj = Instantiate(CityPrefab);
        obj.transform.position = pos;
        obj.GetComponent<City>().Population = pop;
    }

    private void Awake()
    {
        Instance = this;
    }

    Color[] playerColors =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
    };

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        satelliteTimer = 0;
        currentPlayer_ = -1;
        players_ = new List<Player>();

        for (int i = 0; i < StartSettings.numPlayers; i++)
        {
            players_.Add(new Player(StartSettings.names[i], playerColors[i], moneyDisplays[i]));
        }

        for (int i = 0; i < moneyDisplays.Length; i++)
        {
            moneyDisplays[i].gameObject.SetActive(i < StartSettings.numPlayers);
        }


        // Enable for debugging

        //CreateSatellite(new Vector3(7.0f, 3.0f, 0.0f), new Vector2(0.0f, 5.0f), players_[0]);
        //CreateSatellite(new Vector3(10.0f, 3.0f, 0.0f), new Vector2(1.0f, 5.0f), players_[1]);

        //for (int i = 0; i < 20; i++)
        //{
        //    CreateSatellite(new Vector3(6.0f + i, 1.0f, 0.0f), new Vector2(0.0f, 5.0f - (i / 7f)) + UnityEngine.Random.insideUnitCircle * 0.5f, players_[2]);
        //}

        Transform earth = GameObject.Find("Earth")?.transform;
        if (earth == null)
            Debug.LogError("Earth not found!");

        for (int i = 0; i < 10; i++)
        {
            float r = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            Vector3 pos = new Vector3(Mathf.Cos(r), Mathf.Sin(r));

            CreateCity(earth.position + pos * EARTH_RADIUS, UnityEngine.Random.Range(10, 500));
        }

        ButtonNext.onClick.AddListener(TaskNext);
        ButtonCreate.onClick.AddListener(TaskCreate);
        ButtonCancelCreate.onClick.AddListener(TaskCancelCreate);
        ButtonMove.onClick.AddListener(TaskMove);
        ButtonCancelMove.onClick.AddListener(TaskCancelMove);
        HideButton(ButtonCancelCreate);
        HideButton(ButtonCancelMove);
        Pause();
    }

    private void TaskNext()
    {
        Paused = false;

        foreach (GameObject rocket in rockets_)
        {
            rocket.GetComponent<Rocket>().HandleTurn();
        }

        if (handledSatellite_ != null)
        {
            GameObject rocket = handledSatellite_.GetComponent<Physics>().SpawnRocket(EARTH_RADIUS, players_.Count - 1, CurrentPlayer);
            rockets_.Add(rocket);
            players_[currentPlayer_].RemoveSatellite(handledSatellite_);
            Destroy(handledSatellite_);
            handledSatellite_ = null;
        }

        placingSatellite_ = false;
        currentAction = ActionType.None;


        HideButton(ButtonCreate);
        HideButton(ButtonMove);
        HideButton(ButtonNext);
        HideButton(ButtonCancelCreate);
        HideButton(ButtonCancelMove);
        HideButton(ButtonUpgrade);
    }

    private void TaskCreate()
    {
        if (!Paused)
        {

        }
        else if (handledSatellite_ != null)
        {

        }
        else
        {
            ButtonCreate.enabled = false;
            placingSatellite_ = true;
            handledSatellite_ = CreateSatellite(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), players_[currentPlayer_]);
            handledSatellite_.GetComponent<Physics>().Placed = false;
            handledSatellite_.GetComponent<Collider2D>().isTrigger = true;
            ShowButton(ButtonCancelCreate);
            HideButton(ButtonNext);
            HideButton(ButtonCreate);
            HideButton(ButtonMove);
            HideButton(ButtonUpgrade);
            currentAction = ActionType.CreateSatellite;
        }
    }

    private void TaskCancelCreate()
    {
        if (!Paused)
        {

        }
        else
        {
            ButtonCreate.enabled = true;
            placingSatellite_ = false;
            if (handledSatellite_ != null)
            {
                players_[currentPlayer_].RemoveSatellite(handledSatellite_);
                Destroy(handledSatellite_);
                handledSatellite_ = null;
            }
            ShowButton(ButtonNext);
            ShowButton(ButtonCreate);
            ShowButton(ButtonMove);
            ShowButton(ButtonUpgrade);
            HideButton(ButtonCancelCreate);
            HideButton(ButtonCancelMove);
            currentAction = ActionType.None;
        }
    }

    private void TaskMove()
    {
        if (!Paused)
        {

        }
        if (players_[currentPlayer_].SatellitesCount() == 0)
        {

        }
        else
        {

            ShowButton(ButtonCancelMove);
            HideButton(ButtonNext);
            HideButton(ButtonCreate);
            HideButton(ButtonMove);
            HideButton(ButtonUpgrade);
            currentAction = ActionType.MoveSatellite;
        }
    }

    private void TaskCancelMove()
    {
        if (!Paused)
        {

        }
        else
        {
            if (handledSatellite_ != null)
            {
                handledSatellite_.GetComponent<Physics>().Velocity = originalVelocity_;
                handledSatellite_.GetComponent<Physics>().RecalculateParticles();
                handledSatellite_ = null;
            }
            ShowButton(ButtonNext);
            ShowButton(ButtonCreate);
            ShowButton(ButtonMove);
            ShowButton(ButtonUpgrade);
            HideButton(ButtonCancelMove);
            currentAction = ActionType.None;
        }
    }

    private void ShowButton(Button button)
    {
        button.gameObject.SetActive(true);
    }

    private void HideButton(Button button)
    {

        button.gameObject.SetActive(false);
    }

    private void Pause()
    {
        Paused = true;

        if (roundCount == 0)
        {
            Player winner = players_[0];
            for (int i = 1; i < players_.Count; i++)
            {
                if (players_[i].Money > winner.Money)
                {
                    winner = players_[i];
                }
            }

            InfoText.text = winner.Name + " won!";
            StartCoroutine(endGame());
            return;
        }
        satelliteTimer = 0;
        roundCount--;
        roundDisplay.text = "Rounds left: " + roundCount;
        currentPlayer_++;
        if (currentPlayer_ >= players_.Count)
        {
            currentPlayer_ -= players_.Count;
        }
        for (int i = 0; i < rockets_.Count; i++)
        {
            GameObject rocket = rockets_[i];
            if (rocket.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
            {
                Vector3 dest = rocket.GetComponent<Rocket>().dest;
                Vector2 destVelocity = rocket.GetComponent<Rocket>().destVelocity;
                Player player = rocket.GetComponent<Rocket>().player;
                float delta = (rocket.transform.position - dest).magnitude;

                rockets_.Remove(rocket);
                Destroy(rocket);
                i--;

                if (delta < 1)
                {
                    CreateSatellite(dest, destVelocity, player);
                }
            }
        }
        InfoText.text = players_[currentPlayer_].Name + "'s turn!";
        ButtonCreate.enabled = true;
        placingSatellite_ = false;
        handledSatellite_ = null;
        ShowButton(ButtonNext);
        ShowButton(ButtonCreate);
        ShowButton(ButtonUpgrade);
        HideButton(ButtonCancelCreate);
        ShowButton(ButtonMove);
        HideButton(ButtonCancelMove);
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(3f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Unpause()
    {
        InfoText.text = "";
    }

    private bool mouseDown_ = false;

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    Vector2 originalVelocity_ = Vector2.zero;
    bool givingImpulse_ = false;

    void mouseClicked()
    {
        switch (currentAction)
        {
            case ActionType.CreateSatellite:
                if (handledSatellite_ != null && !mouseDown_)
                {

                    if (!IsPointerOverUIObject())
                    {
                        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                        handledSatellite_.transform.position = new Vector3(worldPos.x, worldPos.y, 0.0f);
                        placingSatellite_ = false;
                        handledSatellite_.GetComponent<Physics>().Placed = true;
                        ShowButton(ButtonNext);
                    }
                }
                if (handledSatellite_ != null && mouseDown_ && !placingSatellite_)
                {
                    Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                    float dx = handledSatellite_.transform.position.x - worldPos.x;
                    float dy = handledSatellite_.transform.position.y - worldPos.y;

                    handledSatellite_.GetComponent<Physics>().Velocity = new Vector2(dx, dy);
                    handledSatellite_.GetComponent<Physics>().RecalculateParticles();
                }
                break;
            case ActionType.MoveSatellite:
                Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                if (!mouseDown_)
                {
                    // reset potential previous moved satellite.
                    if (handledSatellite_ != null && !IsPointerOverUIObject())
                    {
                        handledSatellite_.GetComponent<Physics>().Velocity = originalVelocity_;
                        handledSatellite_.GetComponent<Physics>().RecalculateParticles();
                    }
                    GameObject movedSatellite = players_[currentPlayer_].ClosestSatellite(worldPosition);
                    if (movedSatellite == null)
                    { 
                        handledSatellite_ = null;
                    }
                    else if (!IsPointerOverUIObject())
                    {
                        // Start handling the closest satellite.
                        originalVelocity_ = movedSatellite.GetComponent<Physics>().Velocity;
                        handledSatellite_ = movedSatellite;
                        handledSatellite_.GetComponent<Physics>().GiveImpulse(worldPosition, originalVelocity_);

                        ShowButton(ButtonNext);
                        givingImpulse_ = true;
                    }
                }
                if (handledSatellite_ != null && mouseDown_ && givingImpulse_)
                {
                    handledSatellite_.GetComponent<Physics>().GiveImpulse(worldPosition, originalVelocity_);
                }
                break;
            default:
                break;
        }

        mouseDown_ = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Paused)
        {
            if (Input.GetMouseButton(0))
            {
                mouseClicked();
            }
            else
            {
                mouseDown_ = false;
                placingSatellite_ = true;
                givingImpulse_ = false;
            }
        }
        else
        {
            if (roundCount == 0)
            {
                if (satelliteTimer >= FINAL_ROUND_LENGTH)
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
}
