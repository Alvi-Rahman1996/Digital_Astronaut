using UnityEngine;

public class TiltDish : MonoBehaviour
{
    private bool isTilting;
    public float tiltSpeed = 1f; // Speed at which the dish tilts
    public float maxTiltAngle = 30f; // Maximum tilt angle in degrees

    private AudioSource audioSource; // Reference to the AudioSource component

    private Quaternion initialRotation; // Store the initial rotation

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialRotation = transform.localRotation; // Store the initial local rotation
    }

    public void StartTilt()
    {
        isTilting = true;
        audioSource.Play(); // Play the sound when tilting starts
    }

    public void StopTilt()
    {
        isTilting = false;
        audioSource.Stop(); // Stop the sound when tilting stops
    }

    private void Update()
    {
        if (isTilting)
        {
            // Calculate the new tilt angle based on time and speed
            float targetTiltAngle = Mathf.Sin(Time.time * tiltSpeed) * maxTiltAngle;

            // Apply the local rotation around the up axis (left and right)
            transform.localRotation = initialRotation * Quaternion.Euler(0f, targetTiltAngle, 0f);
        }
    }
}
