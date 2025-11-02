using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerTomatos : MonoBehaviour
{
    [Header("Timer Settings")]
    public float levelDuration = 60f;
    public string targetBool = "";
    private float timeLeft;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text tomatoCountText; // <-- New text object for tomato count

    private GlobalManager globalManager;

    void Start()
    {
        timeLeft = levelDuration;
        UpdateUI();

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
            UpdateUI();
        }
        else
        {
            if (globalManager != null)
            {
                // Set the target bool true based on string name
                if (targetBool == "completedTomatos") globalManager.completedTomatos = true;
                else if (targetBool == "completedMeatball") globalManager.completedMeatball = true;
                else if (targetBool == "completedBowl") globalManager.completedBowl = true;
                else if (targetBool == "completedWorms") globalManager.completedWorms = true;
                else if (targetBool == "completedMix") globalManager.completedMix = true;

                Debug.Log($"Timer finished. Set {targetBool} = true");
            }

            SceneManager.LoadScene("KitchenScene");
        }
    }

    private void UpdateUI()
    {
        // --- Timer display ---
        if (timerText != null)
        {
            int totalSeconds = Mathf.CeilToInt(timeLeft);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        // --- Tomato count display ---
        if (tomatoCountText != null && globalManager != null)
        {
            tomatoCountText.text = $"Tomatoes Hit: {globalManager.numTomatosHit}";
        }
    }
}
