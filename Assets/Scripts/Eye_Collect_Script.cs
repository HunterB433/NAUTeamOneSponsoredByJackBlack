using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EyeCollectible : MonoBehaviour
{
    [Header("Who can collect")]
    [SerializeField] private string playerTag = "MainEyeball";

    private bool picked = false;

    // Cache global manager once (static to avoid repeated Find)
    private static GlobalManager globalManager;

    void Awake()
    {
        if (!globalManager)
        {
            globalManager = FindFirstObjectByType<GlobalManager>();
            if (!globalManager)
                Debug.LogWarning("EyeCollectible: GlobalManager not found in scene!");
        }
    }

    void Reset()
    {
        // Ensure pickup is a trigger
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;

        // Stable triggers if you keep a Rigidbody on the eye
        var rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (picked) return;

        // Handle cases where the player collider is on a child object
        Transform t = other.transform;
        bool isPlayer = false;
        while (t != null)
        {
            if (t.CompareTag(playerTag)) { isPlayer = true; break; }
            t = t.parent;
        }
        if (!isPlayer) return;

        picked = true; // guard first

        // Update global counter
        if (globalManager != null)
        {
            globalManager.numEyeBalls++;
            // Debug.Log($"Eyeball picked up. Total: {globalManager.numEyeBalls}");
        }

        // Update HUD
        if (EyeLevelManager.Instance) EyeLevelManager.Instance.OnCollectedOne();

        // Ask the player's controller to play the 3s cry (if present)
        var mover = other.GetComponentInParent<EyeBallMove>();
        if (mover) mover.PlayCollectCry();

        // Remove the eye
        Destroy(gameObject);
    }
}
