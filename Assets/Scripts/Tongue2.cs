using UnityEngine;

public class Tongue2 : MonoBehaviour
{
    public float idleLength = 0.005f;

    public float rotationLo = 70;
    public float rotationHi = 110f;
    public float rotateSpeed = 1.5f;

    public Vector3 rotateAround = Vector3.back;
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
        pivot = transform.parent?.Find("Pivot");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFoodController != null && !playerFoodController.isHoldingFood)
        {
            if(Input.GetKey(growKey))
            {
                ExtendTongue(growRate);
            }
            else
            {
                if (transform.localScale.z > minRotateLength)
                {
                    ExtendTongue(-1f * shrinkRate);
                }
                else
                {
                    float targetAngle = ((rotationHi - rotationLo) * Mathf.Sin(Time.time * rotateSpeed) + (rotationLo + rotationHi)) / 2f;
                    SetAngleX(targetAngle);
                }
            }
        }
    }

    private void SetAngleX(float targetAngle)
    {
        if (pivot != null)
        {
            float angle = targetAngle - transform.rotation.eulerAngles.x;
            transform.RotateAround(pivot.position, rotateAround, angle);
        }
    }

    private void HandleThrowFood(string otherName)
    {
        if (otherName == transform.parent.name)
        {
            SetTongueLength(minRotateLength, rotationLo);
        }
    }

    private void ExtendTongue(float rate)
    {
        Vector3 newScale = transform.localScale + new Vector3(0, 0, rate);
        transform.localScale = newScale;
    }

    public void SetTongueLength(float length, float rotation)
    {
        float deltaLength = length - transform.localScale.z;
        ExtendTongue(deltaLength);
        SetAngleX(rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tongueTag))
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            playerFoodController.SetHoldingFood(true, other.transform);

            SetTongueLength(idleLength, rotationHi);
        }
    }
}
