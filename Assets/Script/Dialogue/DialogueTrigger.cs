using UnityEngine;
using UnityEngine.Events; // Penting!

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    // Ini adalah 'kotak aksi' yang bisa Anda isi di Inspector!
    public UnityEvent OnDialogueFinish;

    public void TriggerDialogue()
    {
        // Sekarang kita passing sebuah Aksi ke Dialogue Manager
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, () => {
            OnDialogueFinish.Invoke();
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}