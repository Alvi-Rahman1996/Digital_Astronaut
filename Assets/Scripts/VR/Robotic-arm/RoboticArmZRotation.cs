using UnityEngine;

public class RoboticArmZRotation : MonoBehaviour
{
    public float rotationSpeed = 15f; // Adjust this value to control the speed of the z-axis rotation

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Invert the rotation amount based on the joystick input
        float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;

        // Apply the rotation to the z-axis of the robotic arm
        transform.Rotate(0f, 0f, rotationAmount);
    }
}
