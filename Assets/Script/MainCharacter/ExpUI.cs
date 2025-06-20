using TMPro;
using UnityEngine;

public class ExpUI : MonoBehaviour
{
    public TextMeshProUGUI expText;

    void Update()
    {
        if (PlayerData.Instance != null)
        {
            expText.text = "EXP: " + PlayerData.Instance.exp;
        }
    }
}
