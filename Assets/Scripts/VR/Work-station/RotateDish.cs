using UnityEngine;

public class RotateDish : MonoBehaviour
{
    private bool isRotating;
    public float rotationSpeed = 15f; // Speed at which the dish rotates

    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Rotate()
    {
        isRotating = true;
        audioSource.Play(); // Play the sound when rotation starts
    }

    public void StopRotation()
    {
        isRotating = false;
        audioSource.Stop(); // Stop the sound when rotation stops
    }

    private void Update()
    {
        if (isRotating)
        {
            // Rotate the dish continuously based on the rotation speed
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
