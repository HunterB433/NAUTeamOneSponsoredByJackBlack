using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EyeCollectible : MonoBehaviour
{
    [SerializeField] private string playerTag = "MainEyeball";

    // Safety: prevent double-collect
    private bool picked = false;

    // Reference to your GlobalManager (DontDestroyOnLoad)
    private GlobalManager globalManager;

    void Awake()
    {
        // Cache the global manager once
        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("GlobalManager not found in scene!");
        }
    }

    void Reset()
    {
        // Make collider a trigger automatically
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;

        // If this object has a Rigidbody, make it kinematic for stable triggers
        var rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (picked) return;
        if (!other.CompareTag(playerTag)) return;

        // Mark as collected first to prevent double-count
        picked = true;

        // >>> Increment eyeball count on the global manager <<<
        if (globalManager != null)
        {
            globalManager.numEyeBalls++;
            Debug.Log($"Eyeball picked up. Total: {globalManager.numEyeBalls}");
        }

        // Update HUD
        if (EyeLevelManager.Instance) EyeLevelManager.Instance.OnCollectedOne();

        // Remove the eye from the scene
        Destroy(gameObject);
    }
}
