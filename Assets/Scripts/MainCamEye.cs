using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonFollowSticky : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public string targetTag = "MainEyeball";

    [Header("Offset (world space)")]
    public Vector3 offset = new Vector3(0f, 5f, -14f);   // farther back = more negative Z
    public float minDistance = 0.6f;                     // how close the camera is allowed to get

    [Header("Smoothing")]
    public float smoothTime = 0.15f;

    [Header("Rotation")]
    public bool lookAtTarget = true;

    [Header("Collision")]
    public LayerMask collisionMask;                      // set to your level/Default
    public float cameraRadius = 0.3f;                    // spherecast radius
    public float collisionBuffer = 0.05f;                // keep a little space from walls

    Quaternion initialRotation;
    Vector3 vel;

    void Awake()
    {
        initialRotation = transform.rotation;
        AcquireTarget();
    }

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    void OnSceneLoaded(Scene s, LoadSceneMode m) => AcquireTarget();

    void AcquireTarget()
    {
        if (target) return;
        var go = GameObject.FindGameObjectWithTag(targetTag);
        if (go) target = go.transform;
    }

    void LateUpdate()
    {
        if (!target) { AcquireTarget(); if (!target) return; }

        // Desired world-space position (no rotation inheritance)
        Vector3 desired = target.position + offset;

        // ---- Collision handling ----
        Vector3 origin = target.position;
        Vector3 toDesired = desired - origin;
        float maxDist = toDesired.magnitude;
        if (maxDist < 0.001f) maxDist = 0.001f;

        Vector3 dir = toDesired / maxDist;

        float targetDist = maxDist; // how far we can place the camera
        if (Physics.SphereCast(origin, cameraRadius, dir, out RaycastHit hit, maxDist, collisionMask, QueryTriggerInteraction.Ignore))
        {
            targetDist = Mathf.Max(minDistance, hit.distance - collisionBuffer);
        }

        Vector3 collisionAdjusted = origin + dir * targetDist;

        // Smooth follow to the collision-adjusted position
        transform.position = Vector3.SmoothDamp(transform.position, collisionAdjusted, ref vel, smoothTime);

        // Rotation
        if (lookAtTarget)
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        else
            transform.rotation = initialRotation;
    }
}
