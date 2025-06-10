using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public Animator animator;

    public int maxHealth = 3;
    public int currentHealth;

    public List<string> items = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Tetap hidup antar scene
        }
        else
        {
            Destroy(gameObject); // Cegah duplikat
        }

        currentHealth = maxHealth;
    }

    public float HealthPercent => (float)currentHealth / maxHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player mati");
            animator.SetBool("IsDead", true);
        }
    }

    public void AddItem(string itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            Debug.Log("Item ditambahkan: " + itemName);
        }
    }
}
