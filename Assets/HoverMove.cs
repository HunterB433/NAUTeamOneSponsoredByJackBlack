using UnityEngine;

public class HoverMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float hoverHeight = 1.5f;
    public float hoverSmooth = 5f;

    [Header("Movement Bounds (XZ)")]
    public Vector2 topLeft = new Vector2(-10f, 10f);
    public Vector2 bottomRight = new Vector2(10f, -10f);

    void Start()
    {
        // If a Rigidbody exists, disable all physics behavior
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    void Update()
    {
        // WASD input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = new Vector3(h, 0f, v).normalized * moveSpeed;

        // Apply movement
        transform.position += move * Time.deltaTime;

        // Clamp position to XZ boundaries
        float clampedX = Mathf.Clamp(transform.position.x, topLeft.x, bottomRight.x);
        float clampedZ = Mathf.Clamp(transform.position.z, bottomRight.y, topLeft.y);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

        // Maintain hover height
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            float targetY = hit.point.y + hoverHeight;
            float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * hoverSmooth);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            // If no ground found, just keep at hoverHeight
            transform.position = new Vector3(transform.position.x, hoverHeight, transform.position.z);
        }
    }
}
