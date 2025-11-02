using UnityEngine;
using System.Collections.Generic;

public class MultiSpawner : MonoBehaviour
{
    public GameObject objectType1; // prefab to spawn 25 times
    public GameObject objectType2; // prefab to spawn 1 time
    public List<Transform> spawnPoints = new List<Transform>(); // 16 possible points

    void Start()
    {
        // copy spawn points list so we can remove used ones
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        // ----- SPAWN 25 OBJECTS -----
        for (int i = 0; i < 25; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Transform chosenPoint = availablePoints[index];

            // choose random upside-down rotation (180° on X or Z)
            Quaternion upsideDownRotation;
            if (Random.value < 0.5f)
                upsideDownRotation = Quaternion.Euler(180f, chosenPoint.rotation.eulerAngles.y, chosenPoint.rotation.eulerAngles.z);
            else
                upsideDownRotation = Quaternion.Euler(chosenPoint.rotation.eulerAngles.x, chosenPoint.rotation.eulerAngles.y, 180f);

            Instantiate(objectType1, chosenPoint.position, upsideDownRotation);

            availablePoints.RemoveAt(index); // remove to avoid duplicates
        }

        // ----- SPAWN FINAL OBJECT -----
        int lastIndex = Random.Range(0, availablePoints.Count);
        Transform lastPoint = availablePoints[lastIndex];

        // same random upside-down rule for last object
        Quaternion lastRotation;
        if (Random.value < 0.5f)
            lastRotation = Quaternion.Euler(180f, lastPoint.rotation.eulerAngles.y, lastPoint.rotation.eulerAngles.z);
        else
            lastRotation = Quaternion.Euler(lastPoint.rotation.eulerAngles.x, lastPoint.rotation.eulerAngles.y, 180f);

        Instantiate(objectType2, lastPoint.position, lastRotation);
    }
}