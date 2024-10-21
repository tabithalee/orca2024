using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public Vector3 holdPosition;
    public float foodDistanceAboveHead = 1.67f;

    public Transform toadTopLeft;
    public Transform toadTopRight;
    public Transform importantToad;
    public float fudgeFactor = 0.3f;

    private FoodInteraction playerFoodController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdPosition = transform.position + Vector3.up * foodDistanceAboveHead;

        playerFoodController = GetComponent<FoodInteraction>();
        if (playerFoodController != null)
        {
            playerFoodController.OnBridgeMoved += OnBridgeMove;
        }
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

    void OnBridgeMove()
    {
        Vector3 bridgeVector = toadTopRight.position - toadTopLeft.position;
        float amountToMove = fudgeFactor * (bridgeVector.y / bridgeVector.x) * Mathf.Abs(transform.position.x - importantToad.position.x);

        transform.position = transform.position + new Vector3(0, amountToMove, 0);
    }
}
