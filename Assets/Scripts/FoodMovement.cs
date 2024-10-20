using UnityEngine;
using System;
using System.Collections;

public class FoodMovement : MonoBehaviour
{
    public float maxHeight = 5f;
    public float minHeight = -5f;
    public float speed = 3f;
    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public float screenBoundX = 10f;
    public float zPosition = -2f;
    private Vector3 startPosition;
    private bool movingRight;

    // Define an event to notify when food should return to the pool
    public event Action<GameObject> OnReturnToPool;

    void OnEnable()
    {
        // Set initial position and random direction
        startPosition = new Vector3(0, Camera.main.orthographicSize, zPosition);
        transform.position = startPosition;

        float targetHeight = UnityEngine.Random.Range(minHeight, maxHeight);
        movingRight = UnityEngine.Random.Range(0, 2) == 0;

        StartCoroutine(MoveAndBob(targetHeight));
    }

    IEnumerator MoveAndBob(float targetHeight)
    {
        Vector3 dropTarget = new Vector3(transform.position.x, targetHeight, zPosition);
        while (Vector3.Distance(transform.position, dropTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dropTarget, speed * Time.deltaTime);
            yield return null;
        }

        startPosition = transform.position;

        while (Mathf.Abs(transform.position.x) < screenBoundX)
        {
            transform.position += new Vector3((movingRight ? 1 : -1) * speed * Time.deltaTime, 0, 0);
            float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        // Trigger the event when the object goes off-screen
        OnReturnToPool?.Invoke(gameObject);
    }
}
