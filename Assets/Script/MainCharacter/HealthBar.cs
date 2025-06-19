using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image playerHpBar;

    private void Update()
    {
        this.SetHealth(PlayerData.Instance.HealthPercent);
    }

    public void SetHealth(float health)
    {
        playerHpBar.fillAmount = health;
    }
}
