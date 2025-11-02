using UnityEngine;
using TMPro;

public class Tomato : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ySpeed = 1f;
    public float xSpeed = 1f;
    public float startX = 0.333f;
    public float endX = -1f;

    [Header("UI Display")]
    [Tooltip("Text object that displays the number of tomatoes hit")]
    public TMP_Text tomatoHitText;  // <-- New inspector variable

    private enum State { Up, Across, Down, Done }
    private State currentState = State.Up;
    private Vector3 targetPos;

    private GlobalManager globalManager;

    void Start()
    {
        // Find the GlobalManager
        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("GlobalManager not found in scene!");
        }

        // Start position (Y = 0)
        transform.position = new Vector3(startX, 0f, transform.position.z);
        targetPos = new Vector3(startX, 0.5f, transform.position.z);

        UpdateTomatoHitUI();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Up:
                MoveTowards(targetPos, ySpeed);
                if (ReachedTarget(targetPos))
                {
                    currentState = State.Across;
                    targetPos = new Vector3(endX, 0.5f, transform.position.z);
                }
                break;

            case State.Across:
                MoveTowards(targetPos, xSpeed);
                if (ReachedTarget(targetPos))
                {
                    currentState = State.Down;
                    targetPos = new Vector3(endX, 0f, transform.position.z);
                }
                break;

            case State.Down:
                MoveTowards(targetPos, ySpeed);
                if (ReachedTarget(targetPos))
                {
                    currentState = State.Done;
                    Debug.Log("Tomato finished movement and will destroy itself.");
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private bool ReachedTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 0.001f;
    }

    void OnMouseDown()
    {
        if (globalManager != null)
        {
            globalManager.numTomatosHit++;
            Debug.Log($"Tomato hit! Total hits: {globalManager.numTomatosHit}");
            UpdateTomatoHitUI();
        }

        Destroy(gameObject);
    }

    private void UpdateTomatoHitUI()
    {
        if (tomatoHitText != null && globalManager != null)
        {
            tomatoHitText.text = $"Tomatoes Hit: {globalManager.numTomatosHit}";
        }
    }
}
