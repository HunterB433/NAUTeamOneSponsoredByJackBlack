using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 8, -10);
    public float followLerp = 5f;
    [SerializeField] string playerTag = "MainEyeball"; // your tag

    void Awake() => AcquireTarget();

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene s, LoadSceneMode m) => AcquireTarget();

    void AcquireTarget()
    {
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag(playerTag);
            target = p ? p.transform : null;
        }
    }

    void LateUpdate()
    {
        if (target == null) { AcquireTarget(); if (target == null) return; }

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * followLerp);
        transform.LookAt(target);
    }
}
