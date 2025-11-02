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

    // ----- WALL SETTINGS -----
    public float leftWall = 100f;
    public float rightWall = 150f;
    public float backWall = 100f;
    public float frontWall = 150f;
    public float bottomWall = 60f;
    public float topWall = 70f;

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

        // random initial bounce direction
        bounceDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
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

        // ----- BOUNCING LOGIC -----
        if (!moveNow) // only bounce when not moving to target
        {
            BounceAround();
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
        float randomY = Random.Range(-shakeAmount, shakeAmount);
        float randomZ = Random.Range(-shakeAmount, shakeAmount);

        transform.Rotate(randomX, randomY, randomZ);

        if (shakeTimer <= 0f)
        {
            isShaking = false;
            transform.rotation = originalRotation;
        }
    }

    void BounceAround()
    {
        transform.position += bounceDirection * bounceSpeed * Time.deltaTime;

        // bounce off walls
        if (transform.position.x <= leftWall || transform.position.x >= rightWall)
            bounceDirection.x = -bounceDirection.x;

        if (transform.position.y <= bottomWall || transform.position.y >= topWall)
            bounceDirection.y = -bounceDirection.y;

        if (transform.position.z <= backWall || transform.position.z >= frontWall)
            bounceDirection.z = -bounceDirection.z;

        // clamp position so it stays inside the room
        float x = Mathf.Clamp(transform.position.x, leftWall, rightWall);
        float y = Mathf.Clamp(transform.position.y, bottomWall, topWall);
        float z = Mathf.Clamp(transform.position.z, backWall, frontWall);
        transform.position = new Vector3(x, y, z);
    }
}