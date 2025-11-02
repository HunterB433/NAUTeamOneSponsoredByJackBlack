using UnityEngine;

public class ClickMoveShakeBounce : MonoBehaviour
{
    // ----- MOVEMENT SETTINGS -----
    public Vector3 targetPosition = new Vector3(122.9f, 64.3f, 119.4f);
    public float speed = 2f;

    // ----- SHAKE SETTINGS -----
    public float shakeAmount = 5f;    // how many degrees to rotate
    public float shakeSpeed = 0.05f;  // how fast between shakes
    public float shakeDuration = 2f;  // how long to shake
    float shakeTimer = 0f;
    bool isShaking = false;
    Quaternion originalRotation;

    // ----- ROOM BOUNDS -----
    public float minX = 100f;
    public float maxX = 150f;
    public float minZ = 100f;
    public float maxZ = 150f;
    public float fixedY = 64f; // height to stay at

    // ----- BOUNCING SETTINGS -----
    public float bounceSpeed = 3f;
    private Vector3 bounceDirection;

    // ----- INTERNAL VARIABLES -----
    Camera mainCam;
    bool moveNow = false;

    void Start()
    {
        mainCam = Camera.main;
        originalRotation = transform.rotation;

        // random initial bounce direction (XZ plane only)
        bounceDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // fix initial Y
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }

    void Update()
    {
        // ----- CLICK DETECTION -----
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    moveNow = true;
                }
            }
        }

        // ----- SMOOTH MOVEMENT TO TARGET -----
        if (moveNow)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            // keep Y fixed
            Vector3 pos = transform.position;
            pos.y = fixedY;
            transform.position = pos;

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moveNow = false;
                StartShake();
            }
        }

        // ----- SHAKING LOGIC -----
        if (isShaking)
        {
            ShakeObject();
        }

        // ----- BOUNCING LOGIC (XZ only) -----
        if (!moveNow) // only bounce when not moving to target
        {
            BounceXZ();
        }
    }

    void StartShake()
    {
        isShaking = true;
        shakeTimer = shakeDuration;
    }

    void ShakeObject()
    {
        shakeTimer -= Time.deltaTime;

        float randomX = Random.Range(-shakeAmount, shakeAmount);
        float randomY = 0f; // don't shake vertically
        float randomZ = Random.Range(-shakeAmount, shakeAmount);

        transform.Rotate(randomX, randomY, randomZ);

        if (shakeTimer <= 0f)
        {
            isShaking = false;
            transform.rotation = originalRotation;
        }
    }

    void BounceXZ()
    {
        Vector3 pos = transform.position;
        pos += bounceDirection * bounceSpeed * Time.deltaTime;
        pos.y = fixedY; // keep Y fixed

        // bounce on X axis
        if (pos.x <= minX || pos.x >= maxX)
            bounceDirection.x = -bounceDirection.x;

        // bounce on Z axis
        if (pos.z <= minZ || pos.z >= maxZ)
            bounceDirection.z = -bounceDirection.z;

        // clamp position inside bounds
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}