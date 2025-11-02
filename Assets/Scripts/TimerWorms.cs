using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerWorms : MonoBehaviour
{
    [Header("Timer Settings")]
    public float levelDuration = 60f;
    private float timeLeft;
    private float updateInterval = 1f;   // Update display every 1 second
    private float updateTimer = 0f;

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text cutsText;
    public TMP_Text accuracyText;

    private GlobalManager globalManager;

    void Start()
    {
        timeLeft = levelDuration;
        UpdateTimerUI();

        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("[TimerWorms] GlobalManager not found in scene!");
        }
    }

    void Update()
    {
        if (timeLeft > 0f)
        {
            // Countdown
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f) timeLeft = 0f;

            // Update once per second
            updateTimer += Time.deltaTime;
            if (updateTimer >= updateInterval)
            {
                updateTimer = 0f;
                UpdateTimerUI();
                UpdateCutsAndAccuracyUI();
            }
        }
        else
        {
            // Time finished
            if (globalManager != null)
            {
                globalManager.completedWorms = true;
                Debug.Log("[TimerWorms] Timer ended. Set completedWorms = true.");
            }

            SceneManager.LoadScene("KitchenScene");
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int totalSeconds = Mathf.CeilToInt(timeLeft);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void UpdateCutsAndAccuracyUI()
    {
        if (globalManager == null) return;

        // Update cuts text
        if (cutsText != null)
        {
            cutsText.text = $"Cuts: {globalManager.numWormsCut}";
        }

        // Calculate accuracy safely (avoid divide-by-zero)
        float accuracy = 0f;
        if (globalManager.numWorms > 0)
        {
            accuracy = (float)globalManager.numWormsCut / globalManager.numWorms;
        }

        if (accuracyText != null)
        {
            accuracyText.text = $"Accuracy: {(accuracy * 100f):0}%";
        }
    }
}
