using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Pause game
        gameOverPanel.SetActive(true);
    }

    public void Victory()
    {
        Time.timeScale = 0f;
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        else
            Debug.Log("Victory! (Tidak ada victoryPanel di-assign)");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main"); // Ganti nama sesuai scene yang kamu gunakan
    }
}
