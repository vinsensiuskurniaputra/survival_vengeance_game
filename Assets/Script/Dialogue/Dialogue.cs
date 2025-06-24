using UnityEngine;

// Definisikan struktur untuk satu baris kalimat, lengkap dengan pembicara dan teksnya.
[System.Serializable]
public class Sentence
{
    public string speakerName;

    [TextArea(3, 10)] // Membuat kotak teks lebih besar di Inspector agar mudah dibaca.
    public string text;
}

// Definisikan struktur untuk sebuah percakapan utuh, yang berisi kumpulan "Sentence".
[System.Serializable]
public class Dialogue
{
    public Sentence[] conversationLines;
}