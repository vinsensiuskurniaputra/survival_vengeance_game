using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expAmount = 1;
    public float pickupRadius = 1.5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= pickupRadius)
        {
            // Tambahkan ke player data
            PlayerData.Instance.AddExp(expAmount); // Bisa ganti ke AddExp jika punya fungsi
            Debug.Log("EXP diambil: " + expAmount);

            Destroy(gameObject); // Hapus EXP dari scene
        }
    }
}
