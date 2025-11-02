using UnityEngine;
using System.Collections.Generic;

public class DetectionZone : MonoBehaviour
{
    private bool wormInZone = false;
    private Collider savedOther;
    public WormMove wormMove;
    [SerializeReference]
    public List<WormMove> wormsInZone = new List<WormMove>();

    private GlobalManager globalManager;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        // Find the global manager in the scene (its marked as DontDestroyOnLoad)
        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("[DetectionZone] GlobalManager not found in scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        WormMove worm = other.GetComponent<WormMove>();
        if (worm != null && !wormsInZone.Contains(worm))
        {
            wormsInZone.Add(worm);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WormMove worm = other.GetComponent<WormMove>();
        if (worm != null && wormsInZone.Contains(worm))
        {
            wormsInZone.Remove(worm);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            HandleWormClick();
        }
    }

    private void HandleWormClick()
    {
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
