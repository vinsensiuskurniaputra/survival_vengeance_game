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
    [Tooltip("Kecepatan munculnya huruf per detik. Angka kecil = cepat.")]
    public float typingSpeed = 0.04f; 
    [Header("Pengaturan Audio")]
    public AudioSource backgroundMusicSource;
    [Range(0, 1)]
    public float volumeSaatDialog = 0.1f;
    public float audioFadeDuration = 1.0f;

    // === Variabel Internal ===
    private Queue<string> sentences;
    private Coroutine typingCoroutine;
    private string currentFullSentence;
    private UnityAction onDialogueFinishAction;
    private float originalVolume;
    private Coroutine audioFadeCoroutine;
    private bool isConversationActive = false;

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
        
        if (!isConversationActive)
        {
            isConversationActive = true; // Tandai sesi percakapan dimulai

            // Blok audio ini HANYA akan berjalan untuk dialog pertama
            if (backgroundMusicSource != null)
            {
                originalVolume = backgroundMusicSource.volume; // Simpan volume ASLI

                if(audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
                audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, volumeSaatDialog, audioFadeDuration));
            }
        }

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

    
    public IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        // Loop akan berjalan selama durasi yang ditentukan
        while (currentTime < duration)
        {
            // Tambah waktu berdasarkan waktu frame
            currentTime += Time.deltaTime; 

            // Hitung volume baru menggunakan Lerp (Linear Interpolation)
            // Lerp akan mencari nilai di antara startVolume dan targetVolume
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);

            // Tunggu frame berikutnya sebelum melanjutkan loop
            yield return null; 
        }

        // Pastikan volume diatur ke nilai target di akhir
        audioSource.volume = targetVolume; 
        audioFadeCoroutine = null;
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

    // Tambahkan fungsi baru ini di mana saja di dalam kelas DialogueManager
    public void EndConversation()
    {
        isConversationActive = false; // Tandai sesi percakapan selesai

        if (backgroundMusicSource != null)
        {
            // Kembalikan volume ke aslinya
            if(audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
            audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, originalVolume, audioFadeDuration));
        }
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
            yield return new WaitForSeconds(typingSpeed);
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
        
        if (onDialogueFinishAction != null)
        {
            onDialogueFinishAction.Invoke();
        }
        
        // Hentikan coroutine pemeriksa yang lama jika ada, lalu mulai yang baru
        StopCoroutine("CheckConversationEnd"); // Hentikan dengan nama string
        StartCoroutine("CheckConversationEnd");
    }

    // Coroutine kecil untuk memeriksa status di frame berikutnya
    IEnumerator CheckConversationEnd()
    {
        // Tunggu satu frame. Ini memberi waktu untuk StartDialogue() berikutnya dipanggil.
        yield return null; 

        // Setelah satu frame, kita cek: Apakah panel dialog masih non-aktif?
        // Jika ya, berarti tidak ada dialog baru yang dipicu. Inilah akhir sebenarnya.
        if (!dialoguePanel.activeSelf && isConversationActive)
        {
            isConversationActive = false; // Reset status

            // Kembalikan volume audio
            if (backgroundMusicSource != null)
            {
                if(audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
                audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, originalVolume, audioFadeDuration));
            }
        }
    }
}