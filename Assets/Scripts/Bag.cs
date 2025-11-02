using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Object To Spawn")]
    public GameObject prefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 1f;
    public float xStart = 0.333f;

    private GlobalManager globalManager;

    void Start()
    {
        // Find the GameManager in the DontDestroyOnLoad scene
        globalManager = FindFirstObjectByType<GlobalManager>();

        if (globalManager == null)
        {
            Debug.LogWarning("GameManager not found in scene!");
        }

        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
    }

    void SpawnObject()
    {
        if (prefab == null) return;

        float y = 0f;
        float z = Random.Range(-5.4f, -4.7f);

        Vector3 spawnPos = new Vector3(xStart, y, z);
        Instantiate(prefab, spawnPos, Quaternion.identity);

        // Increment the GameManager counter
        if (globalManager != null)
        {
            globalManager.numTomatos++;
        }
    }
}
