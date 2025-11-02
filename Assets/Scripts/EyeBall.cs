using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EyeBallMove : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 40f;          // XZ movement speed

    [Header("Extra Gravity")]
    [Tooltip("1 = normal gravity. 2 = twice as strong, etc.")]
    public float gravityScale = 1.5f;  // make gravity stronger

    [Header("Visual Spin")]
    [Tooltip("Degrees per second the eyeball spins around its own up axis.")]
    public float spinSpeed = 360f;     // make the eyeball rotate faster

    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Update()
    {
        // purely visual spin (doesn't affect physics)
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // desired horizontal velocity
        Vector3 desired = new Vector3(x, 0f, z).normalized * speed;

        // keep current Y (gravity), drive XZ
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(desired.x, v.y, desired.z);

        // apply extra gravity (per-object scale)
        if (gravityScale != 1f)
        {
            // add only the *extra* gravity beyond the default
            Vector3 extraGravity = Physics.gravity * (gravityScale - 1f);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }
}
