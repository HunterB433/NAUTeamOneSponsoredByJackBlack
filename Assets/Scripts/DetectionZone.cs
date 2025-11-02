// using UnityEngine;

// public class DetectionZone : MonoBehaviour
// {
//     private bool wormInZone = false;
//     public WormMove wormMove;

//     private void OnTriggerEnter(Collider other)
//     {
//         wormInZone = true;
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         wormInZone = false;
//     }

//     private void Update()
//     {
//         if (somethingInZone && Input.GetMouseButtonDown(0))
//         {
//             if (wormMove != null)
//                 wormMove.speed = 0.0f;
//                 Debug.Log(other.name + " entered the detection zone!");
//         }
//     }

//     // private void OnTriggerExit(Collider other)
//     // {
//     //     // This method is called when another collider leaves the trigger zone.
//     //     Debug.Log(other.name + " left the detection zone!");

//     //     // You can add specific logic here, e.g., deactivate an event.
//     //     if (other.CompareTag("Worm"))
//     //     {
//     //         Debug.Log("Worm left the detection zone!");
//     //         // Perform actions related to worm leaving
//     //     }
//     // }
// }