using UnityEngine;
using System.Collections.Generic;

public class MultiSpawner : MonoBehaviour
{
    public GameObject objectType1; // prefab to spawn 15 times
    public GameObject objectType2; // prefab to spawn 1 time
    public List<Transform> spawnPoints = new List<Transform>(); // 16 possible points

    void Start()
    {
        // copy spawn points list so we can remove used ones
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        // ----- SPAWN 15 OBJECTS -----
        for (int i = 0; i < 25; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Transform chosenPoint = availablePoints[index];

            Instantiate(objectType1, chosenPoint.position, chosenPoint.rotation);

            availablePoints.RemoveAt(index); // remove to avoid duplicates
        }

        // ----- SPAWN 16TH OBJECT -----
        // pick randomly from remaining point
        int lastIndex = Random.Range(0, availablePoints.Count);
        Transform lastPoint = availablePoints[lastIndex];

        Instantiate(objectType2, lastPoint.position, lastPoint.rotation);
    }
}