using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BowlTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float levelDuration = 60f;
    public string targetBool = "";
    private float timeLeft;

    [Header("UI")]
    public TMP_Text timerText;

    private GlobalManager globalManager;

    void Start()
    {
        timeLeft = levelDuration;
        UpdateTimerUI();

        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("GlobalManager not found in scene!");
        }
    }

    void Update()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f) timeLeft = 0f;
            UpdateTimerUI();
        }
        else
        {
            if (globalManager != null)
            {
                // Increase numFails by 2
                globalManager.numFails += 2;

                // Set the target bool
                if (targetBool == "completedBowl")
                    globalManager.completedBowl = false;

                Debug.Log($"Timer finished. Added 2 to numFails. Set {targetBool} = false");
            }

            // Switch scenes
            SceneManager.LoadScene("KitchenScene");

            // Prevent Update from running again after scene load starts
            enabled = false;
        }
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
