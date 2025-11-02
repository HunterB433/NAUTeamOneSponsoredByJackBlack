using UnityEngine;

public class ToEat : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target transform to move toward (e.g., Ghouldon’s mouth or cauldron)")]
    public Transform target;

    [Header("Movement Settings")]
    [Tooltip("Speed at which this object moves toward the target")]
    public float moveSpeed = 3f;

    [Tooltip("Distance threshold to stop moving")]
    public float stopDistance = 0.05f;

    private bool moving = false;

    void Start()
    {
        // Automatically start moving when scene starts
        if (target != null)
        {
            moving = true;
            Debug.Log(name + " started moving toward " + target.name);
        }
        else
        {
            Debug.LogWarning("ToEat: Target not assigned for " + name);
        }
    }

    void Update()
    {
        if (moving && target != null)
        {
            MoveTowardTarget();
        }
    }

    private void MoveTowardTarget()
    {
        // Move smoothly toward the target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Stop when close enough
        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            moving = false;
            Debug.Log(name + " reached the target " + target.name);
        }
    }
}
