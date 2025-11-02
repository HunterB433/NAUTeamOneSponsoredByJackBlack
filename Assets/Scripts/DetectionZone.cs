// using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // This method is called when another collider enters the trigger zone.
        Debug.Log(other.name + " entered the detection zone!");

        // You can add specific logic here, e.g., check for a tag,
        // activate an event, or apply a status effect.
        if (other.CompareTag("Worm"))
        {
            Debug.Log("Worm detected!");
            // Perform actions related to worm detection
        }
    private bool wormInZone = false;
    private Collider savedOther;
    public WormMove wormMove;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered the detection zone!");
        savedOther = other;
        wormInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // This method is called when another collider leaves the trigger zone.
        Debug.Log(other.name + " left the detection zone!");

        // You can add specific logic here, e.g., deactivate an event.
        if (other.CompareTag("Worm"))
        {
            Debug.Log("Worm left the detection zone!");
            // Perform actions related to worm leaving
        }
        wormInZone = false;
    }

    // private void Update()
    // {
    //     if (somethingInZone && Input.GetMouseButtonDown(0))
    //     {
    //         if (wormMove != null)
    //             wormMove.speed = 0.0f;
    //             Debug.Log(other.name + " entered the detection zone!");
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     // This method is called when another collider leaves the trigger zone.
    //     Debug.Log(other.name + " left the detection zone!");

//     //     // You can add specific logic here, e.g., deactivate an event.
//     //     if (other.CompareTag("Worm"))
//     //     {
//     //         Debug.Log("Worm left the detection zone!");
//     //         // Perform actions related to worm leaving
//     //     }
//     // }
// }