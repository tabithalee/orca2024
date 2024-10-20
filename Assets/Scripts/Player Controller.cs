using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public Vector3 holdPosition;
    public float foodDistanceAboveHead = 1.67f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdPosition = transform.position + Vector3.up * foodDistanceAboveHead;
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize move vector
        Vector3 move = Vector3.zero;

        // Check for keys
        if (Input.GetKey(leftKey))
        {
            move += Vector3.left * moveSpeed * Time.deltaTime; // Move left
        }
        if (Input.GetKey(rightKey))
        {
            move += Vector3.right * moveSpeed * Time.deltaTime; // Move right
        }

        // Update player position
        transform.position += move;
        holdPosition += move;
    }
}
