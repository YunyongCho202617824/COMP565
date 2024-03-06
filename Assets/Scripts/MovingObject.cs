using UnityEngine;
public class MovingObject : MonoBehaviour
{
    // Declare private variables to store initial positions, distance, turning point, turning switch, and movement speed.
    float initPositionY;
    float initPositionX;
    public float distance;
    public float turningPoint;
    public bool turnSwitch;
    public float moveSpeed;

    // Define a method named leftRight responsible for handling left and right movement.
    void leftRight()
    {
        // Get the current X position of the object.
        float currentPositionX = transform.position.x;

        // Check if the object has moved to the right beyond the specified distance.
        if (currentPositionX >= initPositionX + distance)
        {
            // If true, set the turning switch to false.
            turnSwitch = false;
        }
        // Check if the object has moved to the left beyond the turning point.
        else if (currentPositionX <= turningPoint)
        {
            // If true, set the turning switch to true.
            turnSwitch = true;
        }

        // Move the object left or right based on the turning switch and movement speed.
        if (turnSwitch)
        {
            transform.position = transform.position + new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
        }
    }

    // Start is called before the first frame update.
    void Start()
    {
        // Check if the object's name is "Moving Object."
        if (gameObject.name == "Moving Object")
        {
            // If true, initialize the initial X position and the turning point.
            initPositionX = transform.position.x;
            turningPoint = initPositionX - distance;
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // Check if the object's name is "Moving Object."
        if (gameObject.name == "Moving Object")
        {
            // If true, call the leftRight method to handle movement.
            leftRight();
        }
    }
}