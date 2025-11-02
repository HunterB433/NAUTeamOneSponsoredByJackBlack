// using UnityEngine;

using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private bool wormInZone = false;
    private Collider savedOther;
    public WormMove wormMove;

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
}