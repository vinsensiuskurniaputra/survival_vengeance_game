using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Penting: Tambahkan ini untuk mengakses komponen Button
using UnityEngine.Events; // Sangat Penting: Untuk sistem event
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // === Referensi UI (dengan tambahan tombol) ===
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button continueButton;

    // === Variabel Internal ===
    private Queue<string> sentences;
    private Coroutine typingCoroutine;
    private string currentFullSentence;
    private UnityAction onDialogueFinishAction;

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);

        // Menghubungkan fungsi ke tombol secara dinamis
        nextButton.onClick.AddListener(OnNextButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }
    
    // Fungsi ini dipanggil dari luar (oleh trigger)
    public void StartDialogue(Dialogue dialogue, UnityAction onFinish)
    {
        dialoguePanel.SetActive(true);
        nextButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        onDialogueFinishAction = onFinish; // Simpan aksi yang harus dijalankan setelah selesai

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    // Fungsi ini terhubung ke OnClick milik NextButton
    public void OnNextButtonClicked()
    {
        // Jika sedang mengetik, selesaikan. Jika tidak, tampilkan kalimat berikutnya.
        if (typingCoroutine != null)
        {
            CompleteSentence();
        }
        else
        {
            DisplayNextSentence();
        }
    }

    // Fungsi ini terhubung ke OnClick milik ContinueButton
    public void OnContinueButtonClicked()
    {
        EndDialogue();
    }
    
    private void DisplayNextSentence()
    {
        // Jika tidak ada kalimat lagi
        if (sentences.Count == 0)
        {
            dialogueText.text = currentFullSentence; // Pastikan kalimat terakhir utuh
            nextButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true); // Tampilkan tombol Selesai
            return;
        }

        currentFullSentence = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(currentFullSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // Tunggu satu frame. Untuk kecepatan, gunakan WaitForSeconds.
        }
        typingCoroutine = null; // Tandai bahwa ketikan selesai
    }

    void CompleteSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueText.text = currentFullSentence;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        // Jalankan aksi yang sudah kita simpan saat dialog dimulai
        if (onDialogueFinishAction != null)
        {
            onDialogueFinishAction.Invoke();
        }
    }
}