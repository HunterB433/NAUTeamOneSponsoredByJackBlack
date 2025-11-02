using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ClickMoveBounce : MonoBehaviour
{
    // ----- REFERENCE TO SCRIPT B -----
    public TeleportAndRotate objectBScript;

    // ----- MOVEMENT SETTINGS -----
    public Vector3 targetPosition = new Vector3(122.9f, 64.3f, 119.4f);
    public float moveSpeed = 2f;

    // ----- ROOM BOUNDS (XZ only) -----
    public float minX = 100f;
    public float maxX = 150f;
    public float minZ = 100f;
    public float maxZ = 150f;
    public float groundY = 64f;

    // ----- BOUNCING SETTINGS -----
    public float bounceSpeed = 3f;
    private Vector3 bounceDirection;

    // ----- AUDIO -----
    [Header("Audio")]
    public AudioSource clickSound;

    // ----- INTERNAL VARIABLES -----
    private Camera mainCam;
    private bool moveNow = false;

    void Start()
    {
        mainCam = Camera.main;

        // random bounce direction (XZ only)
        bounceDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // set to ground height
        Vector3 pos = transform.position;
        pos.y = groundY;
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
                    Debug.Log("Object clicked!");

                    // Play sound immediately on click
                    if (clickSound != null)
                    {
                        clickSound.Play();
                        Debug.Log("Click sound played!");
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
                Debug.Log("YOU WIN!!!");

                // Trigger Script B
                if (objectBScript != null)
                {
                    objectBScript.ActivateSequence();
                    Debug.Log("Triggered Object B!");
                }

                // Update global variable
                if (GlobalManager.Instance != null)
                {
                    GlobalManager.Instance.completedBowl = true;
                    Debug.Log("completedBowl set to TRUE!");
                }

                // Switch scene
                SceneManager.LoadScene("KitchenScene");
            }
        }

        // ----- BOUNCING -----
        if (!moveNow)
        {
            BounceXZ();
        }
    }

    void BounceXZ()
    {
        Vector3 pos = transform.position;
        pos += bounceDirection * bounceSpeed * Time.deltaTime;
        pos.y = groundY;

        if (pos.x <= minX || pos.x >= maxX)
            bounceDirection.x = -bounceDirection.x;
        if (pos.z <= minZ || pos.z >= maxZ)
            bounceDirection.z = -bounceDirection.z;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}