using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSettings : MonoBehaviour
{
    public static int numPlayers = 2;

    public static string[] names = new string[4];

    public Slider slider;
    public Text numDisplay;

    public GameObject[] PlayerNames;

    private void Start()
    {
        slider.value = numPlayers;
        OnSliderChange(numPlayers);
        if (PlayerNames.Length < 4)
            Debug.LogError("Link the player input fields, now!");
    }

    public void OnSliderChange(float value)
    {
        for (int i = 0; i < PlayerNames.Length; i++)
        {
            PlayerNames[i].SetActive(i < value);
        }
        numPlayers = (int)value;
        numDisplay.text = value.ToString();
    }

    public void SetName0(string n) { names[0] = n; }
    public void SetName1(string n) { names[1] = n; }
    public void SetName2(string n) { names[2] = n; }
    public void SetName3(string n) { names[3] = n; }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
