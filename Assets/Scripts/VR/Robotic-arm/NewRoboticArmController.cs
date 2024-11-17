using UnityEngine;

public class NewRoboticArmController : MonoBehaviour
{
    public float movementSpeed = 15f; // Adjust this value to control the speed of the robotic arm movement

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement amount based on the joystick input
        float movementAmount = verticalInput * movementSpeed * Time.deltaTime;

        // Apply the movement to the robotic arm
        transform.Rotate(0f, movementAmount, 0f);
    }
}
