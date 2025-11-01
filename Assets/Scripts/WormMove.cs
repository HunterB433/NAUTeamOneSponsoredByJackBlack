using UnityEngine;

public class WormMove : MonoBehaviour
{
    public float minX = -5f;
    public float maxX = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(transform.position.x > -0.7f || transform.position.x < 0.7f)
        {
            transform.position += Vector3.right * Time.deltaTime; 
        }
        

        // // guhh
        // Vector3 currentPosition = transform.position;
        // currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        // transform.position = currentPosition;

    }
}
