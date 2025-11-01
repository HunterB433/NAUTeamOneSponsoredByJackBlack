using UnityEngine;

public class ClickMove : MonoBehaviour
{
    // ----- ENTER COORDINATES HERE -----
    public Vector3 targetPosition = new Vector3(122.9f, 64.3f, 119.4f);
    public float speed = 2f;   // how fast to move
    Camera mainCam;
    bool moveNow = false;
    bool shakeNow = false;


    float timer = Random.Range(1f, 10f); // between 0.0 and 10.0


    void Start()
    {
        // get the main camera
        mainCam = Camera.main;
    }









    void Update()
    {

        // add time each frame
        timer -= Time.deltaTime;
        Debug.Log("Timer: " + timer);

        // ----- CLICK DETECTION -----
        if (Input.GetMouseButtonDown(0)) // left mouse click
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // if this object is clicked, start moving
                if (hit.transform == transform)
                {
                    moveNow = true;
                }
            }
        }




        // ----- SMOOTH MOVEMENT -----
        if (moveNow)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,       // uses the coordinates you entered above
                speed * Time.deltaTime
            );

            // stop moving when at target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                moveNow = false;
            }
        }
    }
}