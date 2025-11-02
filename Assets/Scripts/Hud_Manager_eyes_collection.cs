using UnityEngine;
using TMPro;

public class EyeLevelManager : MonoBehaviour
{
    // Simple scene-level "singleton" (not persistent)
    public static EyeLevelManager Instance { get; private set; }

    [Header("Timer")]
    public float levelDuration = 60f;       // seconds
    private float timeLeft;

    [Header("Counts")]
    public string pickupTag = "EyeCollect"; // tag used by your collectibles
    public string playerTag = "MainEyeball";
    private int collected;
    private int totalInScene;

    [Header("UI (TextMeshPro)")]
    public TMP_Text countText;              // drag TMP text
    public TMP_Text timerText;              // drag TMP text

    void Awake()
    {
        Instance = this; // scene-scoped
    }

    void Start()
    {
        // Count all pickups at start
        totalInScene = GameObject.FindGameObjectsWithTag(pickupTag).Length;
        collected = 0;
        timeLeft = levelDuration;
        UpdateUI();
    }

    void Update()
    {
        if (timeLeft <= 0f) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        UpdateUI();

        // If you want to end when time hits zero, you can check here.
        // if (timeLeft <= 0f) Debug.Log("Time up!");
    }

    public void OnCollectedOne()
    {
        collected++;
        UpdateUI();
        // if (collected >= totalInScene) Debug.Log("All eyes collected!");
    }

    private void UpdateUI()
    {
        if (countText) countText.text = $"Eyes: {collected}/{totalInScene}";

        if (timerText)
        {
            int sec = Mathf.CeilToInt(timeLeft);
            int m = sec / 60;
            int s = sec % 60;
            timerText.text = $"{m:00}:{s:00}";
        }
    }
}
