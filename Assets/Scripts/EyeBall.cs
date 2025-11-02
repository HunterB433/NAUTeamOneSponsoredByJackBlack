using UnityEngine;

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

    Rigidbody rb;

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
    }

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
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
        Vector3 v = rb.linearVelocity;
        Vector3 desired = moveDir * speed;
        rb.linearVelocity = new Vector3(desired.x, v.y, desired.z);

        // Extra gravity
        if (gravityScale != 1f)
        {
            Vector3 extraGravity = Physics.gravity * (gravityScale - 1f);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }
}
