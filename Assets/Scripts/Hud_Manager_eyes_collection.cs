using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EyeLevelManager : MonoBehaviour
{
    // Simple scene-level "singleton" (not persistent)
    public static EyeLevelManager Instance { get; private set; }

    [Header("Timer")]
    public float levelDuration = 60f; // seconds
    private float timeLeft;

    [Header("Counts")]
    public string pickupTag = "EyeCollect"; // tag used by your collectibles
    public string playerTag = "MainEyeball";
    private int collected;
    private int totalInScene;

    [Header("UI (TextMeshPro)")]
    public TMP_Text countText;              // drag TMP text
    public TMP_Text timerText;              // drag TMP text

    // Add references/guards
    private GlobalManager globalManager;
    private bool levelEnded = false;

    void Awake()
    {
        Instance = this; // scene-scoped
    }

    void Start()
    {
        // Cache GlobalManager (DontDestroyOnLoad)
        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("GlobalManager not found in scene!");
        }

        // Count all pickups at start
        totalInScene = GameObject.FindGameObjectsWithTag(pickupTag).Length;
        collected = 0;
        timeLeft = levelDuration;
        UpdateUI();
    }

    void Update()
    {
        if (levelEnded) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateUI();
            EndLevel();
            return;
        }

        UpdateUI();
    }

    public void OnCollectedOne()
    {
        if (levelEnded) return;
        collected++;
        UpdateUI();
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

    private void EndLevel()
    {
        if (levelEnded) return;
        levelEnded = true;

        // Mark completion on the global manager
        if (globalManager != null)
        {
            globalManager.completedMeatball = true;
            Debug.Log("Level time reached 0. Set completedMeatball = true.");
        }

        // Load the next scene
        Debug.Log("Loading KitchenScene...");
        SceneManager.LoadScene("KitchenScene");
    }
}
