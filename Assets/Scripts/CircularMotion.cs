using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float frozenX;
    private int clickCount = 0;

    private GlobalManager gm;

    void Start()
    {
        frozenX = transform.position.x;

        // Find the GlobalManager (it should be tagged "GlobalManager")
        gm = FindFirstObjectByType<GlobalManager>();
        if (gm == null)
        {
            Debug.LogError("GlobalManager not found in scene!");
        }
    }

    void Update()
    {
        if (orbitingObject == null)
            return;

        // --- Orbiting on Y–Z plane (X stays frozen) ---
        angle += speed * Mathf.Deg2Rad * Time.deltaTime;
        float y = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        orbitingObject.position = new Vector3(frozenX, transform.position.y + y, transform.position.z + z);

        // --- On click ---
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            // Distance and score logic
            if (targetObject != null)
            {
                Vector2 orbPos = new Vector2(orbitingObject.position.y, orbitingObject.position.z);
                Vector2 targetPos = new Vector2(targetObject.position.y, targetObject.position.z);
                float distance = Vector2.Distance(orbPos, targetPos);
                float maxDistance = radius * 2f;
                float percent = Mathf.Clamp01(1f - (distance / maxDistance)) * 100f;

                // Multiply by mixScore from GlobalManager
                if (gm != null)
                {
                    gm.mixScore *= (percent/100);
                }

                Debug.Log($"Distance: {distance:F3} | Weighted Score: {percent:F1}% | Speed: {speed:F1}");
                speed *= speedIncreaseFactor;
            }

            // Ingredient movement logic (XYZ allowed)
            if (!isMoving && currentIndex < ingredients.Count && pot != null)
            {
                StartCoroutine(MoveIngredient(ingredients[currentIndex]));
                currentIndex++;
            }
            else if (currentIndex >= ingredients.Count)
            {
                Debug.Log("All ingredients have been moved into the pot.");
            }

            // After 4 clicks, complete the mix
            if (clickCount >= 4)
            {
                StartCoroutine(FinishMix());
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

    IEnumerator FinishMix()
    {
        Debug.Log("Mix complete! Returning to KitchenScene in 2 seconds...");
        yield return new WaitForSeconds(2f);

        if (gm != null)
        {
            gm.completedMix = true;
            Debug.Log("GlobalManager: completedMix set to true");
        }

        SceneManager.LoadScene("KitchenScene");
    }
}
