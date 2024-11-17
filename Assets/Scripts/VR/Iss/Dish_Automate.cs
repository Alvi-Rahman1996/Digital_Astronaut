using UnityEngine;

public class Dish_Automate : MonoBehaviour
{
    public float tiltSpeed = 1f; // Speed at which the dish tilts
    public float maxTiltAngle = 30f; // Maximum tilt angle in degrees

    private Quaternion initialRotation; // Store the initial rotation

    private void Start()
    {
        initialRotation = transform.localRotation; // Store the initial local rotation
    }

    private void Update()
    {
        // Calculate the new tilt angle based on time and speed
        float targetTiltAngle = Mathf.Sin(Time.time * tiltSpeed) * maxTiltAngle;

        // Apply the local rotation around the up axis (left and right)
        transform.localRotation = initialRotation * Quaternion.Euler(0f, targetTiltAngle, 0f);
    }
}
