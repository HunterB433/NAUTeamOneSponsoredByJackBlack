// using UnityEngine;

using UnityEngine;

public class DetectionZone : MonoBehaviour
{
<<<<<<< Updated upstream
    private bool wormInZone = false;
    private Collider savedOther;
    public WormMove wormMove;
=======

    private AudioSource audioSource;

    [SerializeReference]
    public List<WormMove> wormsInZone = new List<WormMove>();

    private GlobalManager globalManager;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        // Find the global manager in the scene (it�s marked as DontDestroyOnLoad)
        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("[DetectionZone] GlobalManager not found in scene!");
        }
    }
>>>>>>> Stashed changes

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered the detection zone!");
        savedOther = other;
        wormInZone = true;
    }

    private void Update()
    {
        if (wormInZone && Input.GetMouseButtonDown(0))
        {
            if (wormMove != null)
                wormMove.speed = 0.0f;
            Debug.Log(savedOther.name + " entered the detection zone!");
        }
    }
<<<<<<< Updated upstream
}
=======

    private void HandleWormClick()
    {

        audioSource.Play();

        if (wormsInZone.Count == 0 || globalManager == null)
            return;

        // Add +1 per worm to numWormsCut
        globalManager.numWormsCut += wormsInZone.Count;

        // Add +9 total to numWorms
        globalManager.numWorms += 6;

        Debug.Log($"[DetectionZone] Added {wormsInZone.Count} to numWormsCut (now {globalManager.numWormsCut}), +9 to numWorms (now {globalManager.numWorms}).");

        // Trigger each worm
        foreach (WormMove worm in wormsInZone)
        {
            if (worm != null)
                worm.ResetAndIncreaseSpeed();
        }
    }
}
>>>>>>> Stashed changes
