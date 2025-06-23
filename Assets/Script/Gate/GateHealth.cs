using UnityEngine;

public class GateHealth : MonoBehaviour
{
    [Header("Gate Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    
    [Header("Visual Effects")]
    public GameObject destroyEffect;
    
    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;
    public float flashDuration = 0.1f;
    
    private Color originalColor;
    
    void Start()
    {
        currentHealth = maxHealth;
        
        // Get sprite renderer if not assigned
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Gate health: {currentHealth}/{maxHealth}");
        
        // Visual feedback saat terkena damage
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashDamage());
        }
        
        if (currentHealth <= 0)
        {
            DestroyGate();
        }
    }
    
    System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
    
    void DestroyGate()
    {
        // Spawn destroy effect
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        
        Debug.Log("Gate destroyed!");
        
        // Destroy the gate
        Destroy(gameObject);
    }
}