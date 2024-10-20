using UnityEngine;

public class TongueController : MonoBehaviour
{
    public float idleLength = 0.005f;

    public float rotationLo = 70;
    public float rotationHi = 110f;
    public float rotateSpeed = 1.5f;

    public float minRotateLength = 1f;
    public float growRate = 0.005f;
    public float shrinkRate = 0.01f;
    public KeyCode growKey = KeyCode.C;

    public string tongueTag;
    
    private FoodInteraction playerFoodController;
    private Transform pivot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerFoodController = GetComponentInParent<FoodInteraction>();
        if (playerFoodController != null)
        {
            playerFoodController.OnThrowFood += HandleThrowFood;
        }
        pivot = transform.parent.Find("Pivot");
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerFoodController.isHoldingFood)
        {
            if(Input.GetKey(growKey))
            {
                ExtendTongue(growRate);
            }
            else
            {
                if (transform.localScale.y > minRotateLength)
                {
                    ExtendTongue(-1f * shrinkRate);
                }
                else
                {
                    float targetAngle = ((rotationHi - rotationLo) * Mathf.Sin(Time.time * rotateSpeed) + (rotationLo + rotationHi)) / 2f;
                    SetAngleZ(targetAngle);
                }
            }
        }
    }

    private void SetAngleZ(float targetAngle)
    {
        float angle = targetAngle - transform.rotation.eulerAngles.z;
        transform.RotateAround(pivot.position, Vector3.forward, angle);
    }

    private void HandleThrowFood()
    {
        SetTongueLength(minRotateLength, rotationLo);
    }

    private void ExtendTongue(float rate)
    {
        Vector3 newScale = transform.localScale + new Vector3(0, rate, 0);

        Vector3 growDirection = transform.rotation * Vector3.down;
        Vector3 newPosition = transform.localPosition + growDirection * rate;

        transform.localScale = newScale;
        transform.localPosition = newPosition;
    }

    public void SetTongueLength(float length, float rotation)
    {
        float deltaLength = length - transform.localScale.y;
        ExtendTongue(deltaLength);
        SetAngleZ(rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tongueTag))
        {
            playerFoodController.foodTransform = other.transform;
            playerFoodController.SetHoldingFood(true);

            SetTongueLength(idleLength, rotationHi);
        }
    }
}
