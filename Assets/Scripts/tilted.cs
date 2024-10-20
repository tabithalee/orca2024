using UnityEngine;

public class tilted : MonoBehaviour

{
    // Public variables to reference the two end points of the bridge
    public Transform pointA;
    public Transform pointB;

    void Update()
    {
        // Ensure both points are assigned
        if (pointA != null && pointB != null)
        {
            // Calculate the direction from pointA to pointB
            Vector3 direction = pointB.position - pointA.position;

            // Calculate the new rotation required for the bridge to align with the points
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Apply the target rotation to the bridge, preserving local rotation if needed
            transform.rotation = targetRotation * Quaternion.Euler(0, 90, 0); // Keep your X-axis alignment

            // Adjust the Y position to stay between pointA and pointB
            float midpointY = (pointA.position.y + pointB.position.y) / 2;

            // Set the new position, keeping the X and Z axis the same
            transform.position = new Vector3(transform.position.x, midpointY, transform.position.z);
        }
    }
}

