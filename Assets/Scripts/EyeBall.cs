using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class EyeBallMove : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 40f;

    [Header("Extra Gravity")]
    public float gravityScale = 1.5f;

    [Header("Visual Spin")]
    public float spinSpeed = 360f;

    [Header("Camera (optional)")]
    public Transform cameraTransform;   // drag your camera here; falls back to Camera.main

    [Header("Fall-Off (Y threshold)")]
    [Tooltip("If the eyeball's world Y is less than this value, we consider it 'fallen' and switch scenes.")]
    public float fallYThreshold = -2f;
    [SerializeField] private string kitchenSceneName = "KitchenScene";

    private Rigidbody rb;
    private GlobalManager globalManager;
    private bool transitioning = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (!cameraTransform)
        {
            var cam = Camera.main;
            if (cam) cameraTransform = cam.transform;
            else Debug.LogWarning("EyeBallMove: No camera assigned and no MainCamera found. Using world-space controls.");
        }

        globalManager = FindFirstObjectByType<GlobalManager>();
        if (!globalManager)
        {
            Debug.LogWarning("EyeBallMove: GlobalManager not found. Eye count persists only if your manager exists and is DontDestroyOnLoad.");
        }
    }

    void Update()
    {
        // Spin for visuals
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);

        // Simple Y-based fall check (world space)
        if (!transitioning && transform.position.y < fallYThreshold)
        {
            TransitionToKitchen();
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Choose directions
        Vector3 fwd = Vector3.forward;
        Vector3 right = Vector3.right;
        if (cameraTransform)
        {
            fwd = cameraTransform.forward; fwd.y = 0f; fwd.Normalize();
            right = cameraTransform.right;  right.y = 0f; right.Normalize();
        }

        Vector3 moveDir = right * x + fwd * z;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // Drive XZ, keep current Y
        Vector3 v = rb.linearVelocity;                 // <— use velocity (not linearVelocity)
        Vector3 desired = moveDir * speed;
        rb.linearVelocity = new Vector3(desired.x, v.y, desired.z);

        // Extra gravity
        if (gravityScale != 1f)
        {
            Vector3 extraGravity = Physics.gravity * (gravityScale - 1f);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }

    private void TransitionToKitchen()
    {
        transitioning = true;

        int savedCount = globalManager ? globalManager.numEyeBalls : -1;
        Debug.Log($"Eyeball fell below {fallYThreshold}. Saving eyes={savedCount} and loading '{kitchenSceneName}'.");

        SceneManager.LoadScene(kitchenSceneName);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draw a thin gizmo line at the fall Y to help tune it
        Gizmos.color = Color.red;
        Vector3 c = new Vector3(transform.position.x, fallYThreshold, transform.position.z);
        Gizmos.DrawLine(c + Vector3.left * 100f, c + Vector3.right * 100f);
    }
#endif
}
