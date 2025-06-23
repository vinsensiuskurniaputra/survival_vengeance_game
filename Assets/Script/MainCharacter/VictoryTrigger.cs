using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject victoryPanel;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            victoryPanel.SetActive(true);
            Destroy(gameObject); // Optional: hilangkan trigger setelah menang
        }
    }
}
