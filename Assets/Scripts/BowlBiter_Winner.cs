using UnityEngine;
using System.Collections;

public class TeleportAndRotate : MonoBehaviour
{
    public Vector3 teleportLocation = new Vector3(122.9f, 64.3f, 119.4f);
    public float rotationDuration = 1f;

    public void ActivateSequence()
    {
        StartCoroutine(SequenceRoutine());
    }

    IEnumerator SequenceRoutine()
    {
        // 1️⃣ teleport to specified location
        transform.position = teleportLocation;

        // 2️⃣ wait 1 second
        yield return new WaitForSeconds(1f);

        // 3️⃣ rotate smoothly +90° on Z
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, 90f);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotationDuration);
            yield return null;
        }

        // 4️⃣ wait 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // 5️⃣ instead of disappearing, move up 100 units and reset rotation
        transform.position += new Vector3(0, 100f, 0);
        transform.rotation = Quaternion.identity;
    }
}