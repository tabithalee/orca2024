using Unity.VisualScripting;
using UnityEngine;

public class Toad1_Score : MonoBehaviour
{
    // Public variable to reference the child object's Transform
    public Transform childToMove;

    // Public variable for controlling the Y movement
    public float score;

    // Private variable to store the child's starting local position
    private Vector3 initialLocalPosition;

    void Start()
    {
        // Store the initial local position of the child object
        if (childToMove != null)
        {
            initialLocalPosition = childToMove.localPosition;
        }
    }

    void Update()
    {
        // Apply the movement based on the score variable
        if (childToMove != null)
        {
            // Only update the Y position, while keeping the original X and Z
            childToMove.localPosition = new Vector3(initialLocalPosition.x, initialLocalPosition.y + score, initialLocalPosition.z);
        }
    }
}