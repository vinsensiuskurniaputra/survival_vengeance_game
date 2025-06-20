using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Image bossHpBar;
    public Transform boss;
    public Transform player;
    public float showDistance = 10f;

    private CanvasGroup canvasGroup;

    public bool IsDead { get; set; } = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (boss == null || player == null || IsDead)
        {
            canvasGroup.alpha = 0;
            return;
        }

        float distance = Vector2.Distance(boss.position, player.position);
        canvasGroup.alpha = distance <= showDistance ? 1 : 0;

        // Follow boss
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(boss.position + Vector3.up * 1.5f);
        //transform.position = screenPos;
    }

    public void SetHealth(float healthPercent)
    {
        bossHpBar.fillAmount = healthPercent;
    }
}
