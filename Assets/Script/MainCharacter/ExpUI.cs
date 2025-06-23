using TMPro;
using UnityEngine;

public class ExpUI : MonoBehaviour
{
    public TextMeshProUGUI expText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI maxHpText;
    public TextMeshProUGUI speed;

    void Update()
    {
        if (PlayerData.Instance != null)
        {
            expText.text = "EXP: " + PlayerData.Instance.exp;
            attackText.text = PlayerData.Instance.attackPower.ToString();
            maxHpText.text = PlayerData.Instance.maxHealth.ToString();
            speed.text = PlayerData.Instance.moveSpeed.ToString();
        }
    }
}
