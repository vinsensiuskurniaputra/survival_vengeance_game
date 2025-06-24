using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Data Dialog")]
    public Dialogue dialogue;

    [Header("Pengaturan Musik")]
    [Tooltip("Centang ini jika ingin mengganti musik saat trigger ini aktif.")]
    public bool changeMusicOnTrigger = false;
    [Tooltip("Seret file musik baru yang akan diputar ke sini.")]
    public AudioClip newBackgroundMusic;

    [Header("Events")]
    [Tooltip("Aksi yang dijalankan SEKETIKA saat trigger tersentuh (misal: tutup gerbang).")]
    public UnityEvent OnTriggered;
    [Tooltip("Aksi yang dijalankan SETELAH seluruh dialog ini selesai (misal: aktifkan boss).")]
    public UnityEvent OnDialogueFinish;

    private bool hasBeenTriggered = false;

    // Fungsi ini dipanggil dari script lain atau dari event untuk memulai dialog.
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, () => {
            OnDialogueFinish.Invoke();
        });
    }

    // Fungsi bawaan Unity yang berjalan saat ada Collider2D lain masuk ke area trigger.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;

            // 1. Jalankan pergantian musik dulu (jika dicentang).
            if (changeMusicOnTrigger && newBackgroundMusic != null)
            {
                FindObjectOfType<DialogueManager>().ChangeBackgroundMusic(newBackgroundMusic);
            }

            // 2. Jalankan event lainnya yang sudah diatur di Inspector.
            OnTriggered.Invoke();

            // 3. Baru mulai dialognya.
            TriggerDialogue();
        }
    }
}