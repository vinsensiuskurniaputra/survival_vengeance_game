using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // === Referensi UI (diisi dari Inspector) ===
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button continueButton;

    // === Pengaturan (diisi dari Inspector) ===
    [Tooltip("Kecepatan munculnya huruf per detik. Angka kecil = cepat.")]
    public float typingSpeed = 0.04f;
    [Header("Pengaturan Audio")]
    public AudioSource backgroundMusicSource;
    [Range(0, 1)]
    public float volumeSaatDialog = 0.1f;
    public float audioFadeDuration = 1.0f;

    // === Variabel Internal (untuk logika sistem) ===
    private Queue<Sentence> conversationQueue;
    private Coroutine typingCoroutine;
    private string currentFullSentence;
    private UnityAction onDialogueFinishAction;
    private float originalVolume;
    private Coroutine audioFadeCoroutine;
    private bool isChangingMusic = false;
    private bool isConversationActive = false;

    void Start()
    {
        conversationQueue = new Queue<Sentence>();
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    // --- FUNGSI UTAMA ---

    public void StartDialogue(Dialogue dialogue, UnityAction onFinish)
    {
        // Cek jika ini awal percakapan DAN musik tidak sedang diganti
        if (!isConversationActive && !isChangingMusic)
        {
            isConversationActive = true;
            if (backgroundMusicSource != null)
            {
                originalVolume = backgroundMusicSource.volume;
                if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
                audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, volumeSaatDialog, audioFadeDuration));
            }
        }
        else if (!isConversationActive)
        {
            // Jika ini dialog pertama tapi musik sedang diganti, cukup tandai sesi aktif
            isConversationActive = true;
        }

        dialoguePanel.SetActive(true);
        nextButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        onDialogueFinishAction = onFinish;

        conversationQueue.Clear();
        foreach (Sentence line in dialogue.conversationLines)
        {
            conversationQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (conversationQueue.Count == 0)
        {
            dialogueText.text = currentFullSentence;
            nextButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
            return;
        }

        Sentence currentLine = conversationQueue.Dequeue();
        nameText.text = currentLine.speakerName;
        currentFullSentence = currentLine.text;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(currentLine.text));
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        if (onDialogueFinishAction != null)
        {
            onDialogueFinishAction.Invoke();
        }
        StopCoroutine("CheckConversationEnd");
        StartCoroutine("CheckConversationEnd");
    }

    // --- FUNGSI UNTUK TOMBOL ---

    public void OnNextButtonClicked()
    {
        if (typingCoroutine != null)
        {
            CompleteSentence();
        }
        else
        {
            DisplayNextSentence();
        }
    }

    public void OnContinueButtonClicked()
    {
        EndDialogue();
    }

    // --- LOGIKA EFEK KETIKAN ---

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    private void CompleteSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueText.text = currentFullSentence;
    }

    // --- LOGIKA AUDIO ---

    public void ChangeBackgroundMusic(AudioClip newMusic)
    {
        StartCoroutine(FadeAndChangeMusic(newMusic));
    }

    private IEnumerator FadeAndChangeMusic(AudioClip newMusic)
    {
        isChangingMusic = true;

        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
            audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, 0f, audioFadeDuration));
            yield return audioFadeCoroutine;
        }

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = newMusic;
            backgroundMusicSource.Play();
            float targetVolume = (originalVolume > 0) ? originalVolume : 1.0f;
            if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
            audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, targetVolume, audioFadeDuration));
            yield return audioFadeCoroutine;
        }

        isChangingMusic = false;
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
        audioFadeCoroutine = null;
    }

    // --- LOGIKA AKHIR PERCAKAPAN ---

    private IEnumerator CheckConversationEnd()
    {
        yield return null;
        if (!dialoguePanel.activeSelf && isConversationActive)
        {
            isConversationActive = false;
            if (backgroundMusicSource != null)
            {
                if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
                audioFadeCoroutine = StartCoroutine(FadeAudio(backgroundMusicSource, originalVolume, audioFadeDuration));
            }
        }
    }
}