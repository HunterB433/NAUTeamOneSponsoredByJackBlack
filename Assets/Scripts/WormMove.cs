// using UnityEngine;

// public class WormMove : MonoBehaviour
// {
//     public float speed = 1.0f;
//     public Vector3 pointA;
//     public Vector3 pointB;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         pointA = transform.position;
//         pointB = transform.position + new Vector3(1.4f, 0f, 0f);
//     }

//     // Update is called once per frame
//     void Update()
//     {   

//         float time = Mathf.PingPong(Time.time * speed, 1);

//         transform.position = Vector3.Lerp(pointA,  pointB, time);




//         // if(transform.position.x < 0.7f)
//         // {
//         //     transform.rotation = Quaternion.Euler(0, 90, -90);
//         //     transform.position += Vector3.right * Time.deltaTime; 
//         // }

//         // else if (transform.position.x > -0.7f)
//         // {
//         //     transform.rotation = Quaternion.Euler(180, 90, -90);
//         //     transform.position -= Vector3.right * Time.deltaTime; 
//         // }
        

//     }
// }

using UnityEngine;

public class WormMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1.0f;
    public float speedIncreaseFactor = 1.1f;
    public Vector2 randomSpeedRange = new Vector2(0.75f, 1.25f); // random range for speed

    [Header("Movement Points")]
    public Vector3 pointA;
    public Vector3 pointB;

    private float moveTimer;

    void Start()
    {
        // Define movement endpoints
        pointA = transform.position;
        pointB = transform.position + new Vector3(3.5f, 0f, 0f);

        // Randomize initial speed
        speed = Random.Range(randomSpeedRange.x, randomSpeedRange.y);
    }

    void Update()
    {
        moveTimer += Time.deltaTime * speed;
        float t = Mathf.PingPong(moveTimer, 1f);
        transform.position = Vector3.Lerp(pointA, pointB, t);
    }

    // Called externally (e.g., from DetectionZone)
    public void ResetAndIncreaseSpeed()
    {
        // Reset position and timer
        transform.position = pointA;
        moveTimer = 0f;

        // Randomize speed again within range, then apply increase factor
        float randomBaseSpeed = Random.Range(randomSpeedRange.x, randomSpeedRange.y);
        speed = randomBaseSpeed * speedIncreaseFactor;
    }
}
