using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EyeCollectible : MonoBehaviour
{
    [Tooltip("Leave empty to use EyeLevelManager.playerTag")]
    public string playerTagOverride;

    [Header("VFX / SFX (optional)")]
    public GameObject pickupVfx;   // small particle burst prefab
    public AudioClip pickupSfx;
    public float sfxVolume = 0.8f;

    Collider col;
    bool collected = false;

    void Awake()
    {
        col = GetComponent<Collider>();
        // Make sure this collider is a trigger
        col.isTrigger = true;

        // If there is a Rigidbody on this object, make it kinematic for stable triggers
        var rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // Who counts as the player?
        string playerTag = string.IsNullOrEmpty(playerTagOverride)
            ? (EyeLevelManager.Instance ? EyeLevelManager.Instance.playerTag : "MainEyeball")
            : playerTagOverride;

        if (!other.CompareTag(playerTag)) return;

        collected = true;

        // Notify the level manager
        if (EyeLevelManager.Instance) EyeLevelManager.Instance.OnCollectedOne();

        // Optional feedback
        if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);
        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

        // Hide / remove the collectible
        Destroy(gameObject);   // or: Destroy(gameObject);
    }
}
