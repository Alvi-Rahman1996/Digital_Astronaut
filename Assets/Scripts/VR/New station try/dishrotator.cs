using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class dishrotator : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isDishRotating = false;

    public GameObject rotatingDish;
    public InputHelpers.Button buttonToPress = InputHelpers.Button.PrimaryButton; // Change to the desired button
    public float rotationSpeed = 50f; // Adjust as needed

    // Sound related variables
    public AudioClip rotationSound; // Sound clip to play when the dish is rotating
    public AudioSource rotationAudioSource; // Reference to the AudioSource component

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactor = args.interactor;

        // Toggle the dish rotation
        isDishRotating = !isDishRotating;

        UpdateDishRotation();

        // Play the rotation sound
        if (rotationSound != null && rotationAudioSource != null)
        {
            rotationAudioSource.Play();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactor = null;

        // No need to stop the rotation here; it will be handled on the next press

        UpdateDishRotation();
    }

    private void UpdateDishRotation()
    {
        if (isDishRotating)
        {
            // Logic to rotate the dish continuously
            rotatingDish.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        UpdateDishRotation();
    }
}
