using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EyeCollectible : MonoBehaviour
{
    // Use your player tag
    [SerializeField] private string playerTag = "MainEyeball";

    // Safety: prevent double-collect
    private bool picked = false;

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

        picked = true;

        // Update HUD
        if (EyeLevelManager.Instance) EyeLevelManager.Instance.OnCollectedOne();

        // Remove the eye from the scene
        Destroy(gameObject);
    }
}
