using UnityEngine;

public class HoverMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float hoverHeight = 1.5f;
    public float hoverSmooth = 5f;
    public float rotationSpeed = 10f;

    [Header("Movement Bounds (XZ)")]
    public Vector2 bottomRight = new Vector2(-10f, 10f);
    public Vector2 topLeft = new Vector2(10f, -10f);

    private SceneSwitcher currentInteractTarget;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Debug.Log("HoverMove initialized on: " + gameObject.name);
    }

    void Update()
    {
        Vector3 move = Vector3.zero;
        float targetYRot = transform.eulerAngles.y;

        if (Input.GetKey(KeyCode.W))
        {
            move = Vector3.right * moveSpeed;
            targetYRot = 90f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move = Vector3.back * moveSpeed;
            targetYRot = 180f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move = Vector3.left * moveSpeed;
            targetYRot = 270f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move = Vector3.forward * moveSpeed;
            targetYRot = 0f;
        }

        transform.position += move * Time.deltaTime;
        Quaternion targetRot = Quaternion.Euler(0f, targetYRot, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);

        float clampedX = Mathf.Clamp(transform.position.x, topLeft.x, bottomRight.x);
        float clampedZ = Mathf.Clamp(transform.position.z, bottomRight.y, topLeft.y);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            float targetY = hit.point.y + hoverHeight;
            float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * hoverSmooth);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, hoverHeight, transform.position.z);
        }

        // ===== DEBUG INTERACTION SECTION =====
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            if (currentInteractTarget != null)
            {
                Debug.Log("About to switch scene via: " + currentInteractTarget.name);
                currentInteractTarget.SwitchScene();
            }
            else
            {
                Debug.Log("Pressed E, but no active InteractPoint detected.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractPoint"))
        {
            SceneSwitcher s = other.GetComponent<SceneSwitcher>();
            if (s != null)
            {
                currentInteractTarget = s;
                Debug.Log("Entered InteractPoint: " + other.name + " (SceneSwitcher found)");
            }
            else
            {
                Debug.LogWarning("Entered InteractPoint: " + other.name + " but NO SceneSwitcher found!");
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractPoint"))
        {
            Debug.Log("Exited InteractPoint: " + other.name);
            if (currentInteractTarget != null && other.GetComponent<SceneSwitcher>() == currentInteractTarget)
            {
                currentInteractTarget = null;
                Debug.Log("Cleared current interact target.");
            }
        }
    }
}
