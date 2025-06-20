using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel;

    public TextMeshProUGUI feedbackText; // optional: untuk notifikasi

    public void ToggleUpgradeMenu()
    {
        upgradePanel.SetActive(!upgradePanel.activeSelf);
    }

    public void UpgradeMaxHP()
    {
        int cost = 2;
        if (PlayerData.Instance.SpendExp(cost))
        {
            PlayerData.Instance.maxHealth += 10;
            PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
            feedbackText.text = "Max HP increased!";
        }
        else
        {
            feedbackText.text = "Not enough EXP!";
        }
    }

    public void UpgradeAttack()
    {
        int cost = 1;
        if (PlayerData.Instance.SpendExp(cost))
        {
            PlayerData.Instance.attackPower += 2;
            feedbackText.text = "Attack Power increased!";
        }
        else
        {
            feedbackText.text = "Not enough EXP!";
        }
    }

    public void HealFull()
    {
        int cost = 1;
        if (PlayerData.Instance.SpendExp(cost))
        {
            PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
            feedbackText.text = "Fully healed!";
        }
        else
        {
            feedbackText.text = "Not enough EXP!";
        }
    }
}
