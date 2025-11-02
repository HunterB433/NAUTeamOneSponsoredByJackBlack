using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; private set; }

    [Header("General")]
    public int numGamesCompleted = 0; // 0–5

    [Header("Mun Meatball")]
    public int numEyeBalls = 0;       // 0–11
    public bool completedMeatball = false;

    [Header("Warren Bowl")]
    public int numFails = 0;          // 0–2
    public bool completedBowl = false;

    [Header("Brody Worms")]
    public int numWorms = 0;          // 0–100
    public int numWormsCut = 0;       // 0–100
    public bool completedWorms = false;

    [Header("Hunter Tomatos")]
    public int numTomatos = 0;        // 0–100
    public int numTomatosHit = 0;     // 0-100
    public bool completedTomatos = false;

    [Header("Hunter Mix")]
    public float mixScore = 0f;       // 0–100
    public float placementScore = 0f; // 0–100
    public bool completedMix = false;

    private void Awake()
    {
        // Singleton logic: prevent duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Optional helpers
    public void ResetAllData()
    {
        numGamesCompleted = 0;

        numEyeBalls = 0;
        completedMeatball = false;

        numFails = 0;
        completedBowl = false;

        numWorms = 0;
        numWormsCut = 0;
        completedWorms = false;

        numTomatos = 0;
        numTomatosHit = 0;
        completedTomatos = false;

        mixScore = 0f;
        placementScore = 0f;
        completedMix = false;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
