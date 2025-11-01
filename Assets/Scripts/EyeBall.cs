using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EyeBallMove : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;   // try 8–12

    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // desired horizontal velocity
        Vector3 desired = new Vector3(x, 0f, z).normalized * speed;

        // keep gravity on Y, drive XZ
        rb.linearVelocity = new Vector3(desired.x, rb.linearVelocity.y, desired.z);
    }
}
