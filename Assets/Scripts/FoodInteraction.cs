using System.Collections;
using UnityEngine;

public class FoodInteraction : MonoBehaviour
{
    public bool isHoldingFood = false;
    public Transform foodTransform;

    public Transform toadLeft;
    public Transform toadRight;

    public float throwDuration = 1.0f;
    public KeyCode throwKey = KeyCode.X;

    public delegate void ThrowFoodEventHandler();
    public event ThrowFoodEventHandler OnThrowFood;

    private PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
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

                isHoldingFood = false;

                TriggerThrowFoodEvent();
            }
        }
    }

    public void SetHoldingFood(bool holding)
    {
        isHoldingFood = holding;
    }

    public void TriggerThrowFoodEvent()
    {
        OnThrowFood?.Invoke();
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

        // destroy the food item
        Destroy(foodTransform.gameObject);
    }
}
