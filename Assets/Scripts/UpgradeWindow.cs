using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    public Image thrustFillBar;
    public Image commsFillBar;
    public Image armorFillBar;

    Player player;

    const int upgradeCostFactor = 10000;

    public void Init(Player player)
    {
        this.player = player;
    }

    private void Update()
    {
        thrustFillBar.fillAmount = (thrustFillBar.fillAmount + ((player.thrust - 1f) / 3f)) * 0.5f;
        commsFillBar.fillAmount = (commsFillBar.fillAmount + ((player.comms - 1f) / 3f)) * 0.5f;
        armorFillBar.fillAmount = (armorFillBar.fillAmount + ((player.armor - 1f) / 3f)) * 0.5f;
    }

    public void UpgradeThrust()
    {
        int cost = player.thrust * upgradeCostFactor;
        if (player.thrust < 3 &&  player.Money > cost)
        {
            player.Money -= cost;
            player.thrust++;
        }
    }

    public void UpgradeComms()
    {
        int cost = player.comms * upgradeCostFactor;
        if (player.comms < 3 && player.Money > cost)
        {
            player.Money -= cost;
            player.comms++;
        }
    }

    public void UpgradeArmor()
    {
        int cost = player.armor * upgradeCostFactor;
        if (player.armor < 3 && player.Money > cost)
        {
            player.Money -= cost;
            player.armor++;
        }
    }
}
