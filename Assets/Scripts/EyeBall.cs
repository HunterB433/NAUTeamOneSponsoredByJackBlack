using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
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
    public float fallYThreshold = -2f;
    [SerializeField] private string kitchenSceneName = "KitchenScene";

    [Header("Audio on Collect")]
    [Tooltip("Clip played when an eye is collected (3 seconds). If null, uses the AudioSource's clip.")]
    public AudioClip collectCryClip;
    public float collectCrySeconds = 3f;

    private Rigidbody rb;
    private GlobalManager globalManager;
    private bool transitioning = false;

    private AudioSource audioSrc;
    private Coroutine cryRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
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
            Debug.LogWarning("EyeBallMove: GlobalManager not found. Eye count will only persist if your manager exists and is DontDestroyOnLoad.");
    }

    void Update()
    {
        // Visual spin
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);

        // Fall check
        if (!transitioning && transform.position.y < fallYThreshold)
            TransitionToKitchen();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 fwd = Vector3.forward, right = Vector3.right;
        if (cameraTransform)
        {
            fwd = cameraTransform.forward; fwd.y = 0f; fwd.Normalize();
            right = cameraTransform.right; right.y = 0f; right.Normalize();
        }

        Vector3 moveDir = right * x + fwd * z;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        Vector3 v = rb.linearVelocity;
        Vector3 desired = moveDir * speed;
        rb.linearVelocity = new Vector3(desired.x, v.y, desired.z);

        if (gravityScale != 1f)
        {
            Vector3 extraGravity = Physics.gravity * (gravityScale - 1f);
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }

    // Called by EyeCollectible on the Eye object
    public void PlayCollectCry()
    {
        if (cryRoutine != null) StopCoroutine(cryRoutine);
        cryRoutine = StartCoroutine(PlayCollectCryForSeconds(Mathf.Max(0f, collectCrySeconds)));
    }

    IEnumerator PlayCollectCryForSeconds(float seconds)
    {
        if (!audioSrc) yield break;

        bool oldLoop = audioSrc.loop;
        AudioClip oldClip = audioSrc.clip;

        if (collectCryClip) audioSrc.clip = collectCryClip;

        audioSrc.loop = false;
        audioSrc.time = 0f;
        audioSrc.Play();

        yield return new WaitForSeconds(seconds);

        audioSrc.Stop();
        audioSrc.loop = oldLoop;
        audioSrc.clip = oldClip;
        cryRoutine = null;
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
        Gizmos.color = Color.red;
        Vector3 c = new Vector3(transform.position.x, fallYThreshold, transform.position.z);
        Gizmos.DrawLine(c + Vector3.left * 100f, c + Vector3.right * 100f);
    }
#endif
}
