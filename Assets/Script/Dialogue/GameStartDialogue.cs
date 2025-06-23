using UnityEngine;
using UnityEngine.Events;

public class GameStartDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    public UnityEvent OnDialogueFinish;

    void Start()
    {
        // Panggil dialog setelah beberapa saat agar game sempat 'bernapas'
        Invoke("TriggerDialogue", 0.5f);
    }

    void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, () => {
            OnDialogueFinish.Invoke();
        });
    }
}