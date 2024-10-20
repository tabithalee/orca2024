using System.Collections;
using UnityEngine;

public class FoodInteraction : MonoBehaviour
{
    public bool isHoldingFood = false;
    public PlayerController player;
    public Transform foodTransform;

    public Transform toadLeft;
    public Transform toadRight;

    public float throwDuration = 1.0f;
    public KeyCode throwKey = KeyCode.X;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingFood)
        {
            foodTransform.position = player.holdPosition;

            if (Input.GetKey(throwKey))
            {
                StartCoroutine(ThrowFood());
            }
        }
    }

    private IEnumerator ThrowFood()
    {
        // Save the starting position
        Vector3 startingPosition = foodTransform.position;
        Vector3 endPosition;
        if (startingPosition.x > 0)
        {
            endPosition = toadRight.position;
        }
        else
        {
            endPosition = toadLeft.position;
        }
        
        float elapsedTime = 0f;
        while (elapsedTime < throwDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / throwDuration;

            // Use Lerp for linear interpolation between start and target position
            Vector3 lerpPoint = Vector3.Lerp(startingPosition, endPosition, t);

            // Add height for the curve
            float lerpHeight = startingPosition.y - endPosition.y;
            lerpPoint.y += Mathf.Sin(t * Mathf.PI) * lerpHeight;

            // Update position
            foodTransform.position = lerpPoint;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact target position at the end
        foodTransform.position = endPosition;
    }
}
