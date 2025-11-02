using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
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
