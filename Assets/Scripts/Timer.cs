using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float levelDuration = 60f;   // Total time in seconds
    private float timeLeft;

    [Header("UI")]
    public TMP_Text timerText;          // Assign a TextMeshPro text object in the Inspector

    void Start()
    {
        timeLeft = levelDuration;
        UpdateTimerUI();
    }

    void Update()
    {
        if (timeLeft <= 0f)
        SceneManager.LoadScene("KitchenScene");

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText)
        {
            int totalSeconds = Mathf.CeilToInt(timeLeft);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
