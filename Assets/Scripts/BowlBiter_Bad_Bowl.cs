using UnityEngine;

public class ClickMoveShakeBounce : MonoBehaviour
{
    // ----- MOVEMENT SETTINGS -----
    public Vector3 targetPosition = new Vector3(122.9f, 64.3f, 119.4f);
    public float moveSpeed = 2f;

    [Header("Shake Audio")]
    public AudioClip shakeClip;           // optional; if null uses AudioSource.clip
    public bool loopWhileShaking = true;  // continuous vs one-shot
    AudioSource audioSrc;

    // ----- SHAKE SETTINGS -----
    public float shakeAmount = 5f;
    public float shakeSpeed = 0.05f;
    public float minShakeDuration = 0.2f;
    public float maxShakeDuration = 0.6f;
    public float shakeInterval = 3f;

    float shakeTimer = 0f;
    float shakeCooldown = 0f;
    bool isShaking = false;
    Quaternion originalRotation;

    // ----- ROOM BOUNDS -----
    public float minX = 100f;
    public float maxX = 150f;
    public float minZ = 100f;
    public float maxZ = 150f;
    public float groundY = 64f;

    // ----- BOUNCING -----
    public float bounceSpeed = 3f;
    private Vector3 bounceDirection;

    // ----- INTERNAL -----
    Camera mainCam;
    bool moveNow = false;

    // ----- GLOBAL MANAGER -----
    private GlobalManager globalManager;

    void Start()
    {
        
        mainCam = Camera.main;
        originalRotation = transform.rotation;

        // random bounce direction (XZ only)
        bounceDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // keep at ground height
        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;

        // randomize first shake
        shakeCooldown = Random.Range(1f, shakeInterval);
        audioSrc = GetComponent<AudioSource>();
        if (!audioSrc) audioSrc = gameObject.AddComponent<AudioSource>();

        audioSrc.playOnAwake = false;
        if (shakeClip) audioSrc.clip = shakeClip;
        audioSrc.loop = loopWhileShaking;


        // find global manager
        globalManager = FindFirstObjectByType<GlobalManager>();
        if (globalManager == null)
        {
            Debug.LogWarning("GlobalManager not found in scene!");
        }
    }

    void Update()
    {
        // passive shaking timer
        if (!isShaking)
        {
            shakeCooldown -= Time.deltaTime;
            if (shakeCooldown <= 0f)
            {
                StartShake();
                shakeCooldown = shakeInterval;
            }
        }

        // ----- CLICK DETECTION -----
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    moveNow = true;

                    // increment numFails here
                    if (globalManager != null)
                    {
                        globalManager.numFails++;
                        Debug.Log($"Fail triggered! Total fails: {globalManager.numFails}");
                    }
                }
            }
        }

        // ----- MOVE TO TARGET -----
        if (moveNow)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moveNow = false;
            }
        }

        // ----- PASSIVE SHAKE -----
        if (isShaking)
        {
            ShakeObject();
        }

        // ----- BOUNCING -----
        if (!moveNow)
        {
            BounceXZ();
        }
    }

    void StartShake()
    {
        isShaking = true;
        shakeTimer = Random.Range(minShakeDuration, maxShakeDuration);
        // ---- start sound ----
        if (audioSrc)
        {
            if (loopWhileShaking)
            {
                // continuous sound while shaking
                if (!audioSrc.isPlaying) audioSrc.Play();
            }
            else
            {
                // one tick per shake
                if (shakeClip) audioSrc.PlayOneShot(shakeClip);
                else if (audioSrc.clip) audioSrc.PlayOneShot(audioSrc.clip);
            }
        }
    }

    void ShakeObject()
    {
        shakeTimer -= Time.deltaTime;

        float randomX = Random.Range(-shakeAmount, shakeAmount);
        float randomZ = Random.Range(-shakeAmount, shakeAmount);

        transform.Rotate(randomX, 0f, randomZ);

        if (shakeTimer <= 0f)
        {
            isShaking = false;
            transform.rotation = originalRotation;

            if (audioSrc && loopWhileShaking) audioSrc.Stop();
        }
    }

    void BounceXZ()
    {
        Vector3 pos = transform.position;
        pos += bounceDirection * bounceSpeed * Time.deltaTime;
        pos.y = groundY;

        // bounce off walls
        if (pos.x <= minX || pos.x >= maxX)
            bounceDirection.x = -bounceDirection.x;

        if (pos.z <= minZ || pos.z >= maxZ)
            bounceDirection.z = -bounceDirection.z;

        // clamp inside room
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}
