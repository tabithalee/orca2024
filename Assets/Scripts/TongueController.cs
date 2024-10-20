using UnityEngine;

public class TongueController : MonoBehaviour
{
    public float rotationLo = 70;
    public float rotationHi = 110f;
    public float rotateSpeed = 1.5f;

    public float idleLength = 1f;
    public float growRate = 0.005f;
    public float shrinkRate = 0.01f;
    public KeyCode growKey = KeyCode.C;
    
    private FoodInteraction playerFoodController;
    private Transform pivot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerFoodController = GetComponentInParent<FoodInteraction>();
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
                if (transform.localScale.y > idleLength)
                {
                    ExtendTongue(-1f * shrinkRate);
                }
                else
                {
                    float targetAngle = ((rotationHi - rotationLo) * Mathf.Sin(Time.time * rotateSpeed) + (rotationLo + rotationHi)) / 2f;
                    float angle = targetAngle - transform.rotation.eulerAngles.z;
                    transform.RotateAround(pivot.position, Vector3.forward, angle);
                }
            }
        }
    }

    private void ExtendTongue(float rate)
    {
        Vector3 newScale = transform.localScale + new Vector3(0, rate, 0);

        Vector3 growDirection = transform.rotation * Vector3.down;
        Vector3 newPosition = transform.localPosition + growDirection * rate;

        transform.localScale = newScale;
        transform.localPosition = newPosition;
    }
}
