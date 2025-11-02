using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleMover : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform orbitingObject;
    public Transform targetObject;
    public float radius = 0.55f;
    public float speed = 90f;
    public float speedIncreaseFactor = 1.25f;

    [Header("Ingredient Movement")]
    [Tooltip("List of ingredient GameObjects to move into the pot")]
    public List<GameObject> ingredients = new List<GameObject>();

    [Tooltip("The pot GameObject where ingredients will move to")]
    public Transform pot;

    [Tooltip("Speed of ingredient movement")]
    public float moveSpeed = 3f;

    private float angle;
    private int currentIndex = 0;
    private bool isMoving = false;

    void Update()
    {
        if (orbitingObject == null)
            return;

        // Move orbiting object in a circular path (X-Y plane)
        angle += speed * Mathf.Deg2Rad * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        orbitingObject.position = transform.position + new Vector3(x, y, 0f);

        // On click
        if (Input.GetMouseButtonDown(0))
        {
            // Distance and speed increase logic
            if (targetObject != null)
            {
                Vector2 orbPos = new Vector2(orbitingObject.position.x, orbitingObject.position.y);
                Vector2 targetPos = new Vector2(targetObject.position.x, targetObject.position.y);
                float distance = Vector2.Distance(orbPos, targetPos);
                float maxDistance = radius * 2f;
                float percent = Mathf.Clamp01(1f - (distance / maxDistance)) * 100f;
                Debug.Log($"Distance: {distance:F3} | Score: {percent:F1}% | Speed: {speed:F1}");
                speed *= speedIncreaseFactor;
            }

            // Ingredient movement logic
            if (!isMoving && currentIndex < ingredients.Count && pot != null)
            {
                StartCoroutine(MoveIngredient(ingredients[currentIndex]));
                currentIndex++;
            }
            else if (currentIndex >= ingredients.Count)
            {
                Debug.Log("All ingredients have been moved into the pot.");
            }
        }
    }

    IEnumerator MoveIngredient(GameObject ingredient)
    {
        isMoving = true;
        Debug.Log($"Starting move for ingredient {ingredient.name}...");

        Vector3 startPos = ingredient.transform.position;
        Vector3 targetPos = pot.position;
        float t = 0f;

        // Smooth linear move
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            ingredient.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        ingredient.transform.position = targetPos;
        Debug.Log($"Finished moving {ingredient.name} into the pot.");
        isMoving = false;
    }
}
