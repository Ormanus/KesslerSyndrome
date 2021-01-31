using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    public Image thrustFillBar;
    public Image commsFillBar;
    public Image armorFillBar;

    public Text moneyDisplay;

    Player player;

    const int upgradeCostFactor = 50000;

    void OnEnable()
    {
        player = Game.CurrentPlayer;
        moneyDisplay.text = "Money " + player.Money;
    }

    private void FixedUpdate()
    {
        thrustFillBar.fillAmount = (thrustFillBar.fillAmount + ((player.thrust - 1f) / 3f)) * 0.5f;
        commsFillBar.fillAmount = (commsFillBar.fillAmount + ((player.comms - 1f) / 3f)) * 0.5f;
        armorFillBar.fillAmount = (armorFillBar.fillAmount + ((player.armor - 1f) / 3f)) * 0.5f;
    }

    public void UpgradeThrust()
    {
        int cost = player.thrust * upgradeCostFactor;
        if (player.thrust < 4 &&  player.Money > cost)
        {
            player.Money -= cost;
            player.thrust++;
            moneyDisplay.text = "Money " + player.Money;
        }
    }

    public void UpgradeComms()
    {
        int cost = player.comms * upgradeCostFactor;
        if (player.comms < 4 && player.Money > cost)
        {
            player.Money -= cost;
            player.comms++;
            moneyDisplay.text = "Money " + player.Money;
        }
    }

    public void UpgradeArmor()
    {
        int cost = player.armor * upgradeCostFactor;
        if (player.armor < 4 && player.Money > cost)
        {
            player.Money -= cost;
            player.armor++;
            moneyDisplay.text = "Money " + player.Money;
        }
    }
}
