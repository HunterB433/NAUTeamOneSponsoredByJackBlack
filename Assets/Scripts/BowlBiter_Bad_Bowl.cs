using UnityEngine;

public class ClickMoveAndShake : MonoBehaviour
{
    // ----- MOVEMENT SETTINGS -----
    public Vector3 targetPosition = new Vector3(122.9f, 64.3f, 119.4f);
    public float speed = 2f;

    // ----- SHAKE SETTINGS -----
    public float shakeAmount = 5f;   // how many degrees to rotate
    public float shakeSpeed = 0.05f; // how fast between shakes
    public float shakeDuration = 2f; // how long to shake

    float shakeTimer = 0f;
    bool isShaking = false;

    // ----- INTERNAL VARIABLES -----
    Camera mainCam;
    bool moveNow = false;
    float timer;

    void Start()
    {
        mainCam = Camera.main;
        timer = Random.Range(1f, 10f);
    }

    void Update()
    {
        // timer countdown
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            // start shaking when timer runs out
            StartShake();
            timer = Random.Range(4f, 8f); // wait again before next shake
        }

        // click detection
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

        // smooth movement
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
            }
        }

        // shaking logic
        if (isShaking)
        {
            ShakeObject();
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
            transform.rotation = Quaternion.identity; // reset to normal rotation
        }
    }
}