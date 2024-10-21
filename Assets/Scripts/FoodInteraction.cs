using System.Collections;
using System;
using UnityEngine;

public class FoodInteraction : MonoBehaviour
{
    public bool isHoldingFood = false;
    public Transform foodTransform;

    public Transform toadLeft;
    public Transform toadRight;

    public float throwDuration = 1.0f;
    public KeyCode throwKey = KeyCode.X;

    public delegate void ThrowFoodEventHandler(string otherName);
    public event ThrowFoodEventHandler OnThrowFood;

    // Define an event to notify when food should return to the pool
    public event Action<GameObject> OnReturnToPool;

    // Define the delegate (if using non-generic types).
    public delegate void BridgeMovedEventHandler();

    // Define the event using the delegate.
    public event BridgeMovedEventHandler OnBridgeMoved;

    private PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingFood && foodTransform != null)
        {
            foodTransform.position = player.holdPosition;

            if (Input.GetKey(throwKey))
            {
                StartCoroutine(ThrowFood());

                TriggerThrowFoodEvent();
                isHoldingFood = false;
            }
        }
    }

    public void SetHoldingFood(bool holding, Transform food)
    {
        food.gameObject.GetComponent<FoodMovement>().isHeld = true;
        foodTransform = food.transform;
        isHoldingFood = holding;
    }

    public void TriggerThrowFoodEvent()
    {
        OnThrowFood?.Invoke(player.name);
    }

    private IEnumerator ThrowFood()
    {
        // Save the starting position
        Vector3 startingPosition = foodTransform.position;
        
        Transform fedToad;
        if (startingPosition.x > 0)
        {
            fedToad = toadRight;
        }
        else
        {
            fedToad = toadLeft;
        }

        Vector3 endPosition = fedToad.position;
        
        float elapsedTime = 0f;
        while (elapsedTime < throwDuration && foodTransform != null)
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

        fedToad.gameObject.GetComponent<Toad1_Score>().score += 0.5f;

        OnBridgeMoved?.Invoke();

        // Trigger the event when the object goes off-screen
        OnReturnToPool?.Invoke(foodTransform.gameObject);
    }
}
