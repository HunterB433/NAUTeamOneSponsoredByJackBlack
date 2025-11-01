using UnityEngine;

public class WormMove : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 pointA;
    public Vector3 pointB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointA = transform.position;
        pointB = transform.position + new Vector3(1.4f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {   

        float time = Mathf.PingPong(Time.time * speed, 1);

        transform.position = Vector3.Lerp(pointA,  pointB, time);




        // if(transform.position.x < 0.7f)as
        // {
        //     transform.rotation = Quaternion.Euler(0, 90, -90);
        //     transform.position += Vector3.right * Time.deltaTime; 
        // }

        // else if (transform.position.x > -0.7f)
        // {
        //     transform.rotation = Quaternion.Euler(180, 90, -90);
        //     transform.position -= Vector3.right * Time.deltaTime; 
        // }
        

    }
}
