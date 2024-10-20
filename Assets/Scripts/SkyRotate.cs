using UnityEngine;

public class SkyRotate : MonoBehaviour
{
    // Public variable to control the rotation speed (can be modified in the editor)
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotate around the Y axis at the specified speed
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
