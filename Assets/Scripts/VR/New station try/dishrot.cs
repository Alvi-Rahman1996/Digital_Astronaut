using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class dishrot : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private Renderer buttonRenderer;
    private bool isObjectRotating;
    private bool isButtonPressed = false;

    public GameObject rotatingObject;
    public InputHelpers.Button buttonToPress = InputHelpers.Button.PrimaryButton; // Change to the desired button
    public float rotationSpeed = 50f; // Adjust as needed
    public Color startButtonColor = Color.red;
    public Color pressedButtonColor = Color.green;
    public Renderer objectRendererToChange; // Reference to the Renderer component of the GameObject

    private Material[] objectMaterials;
    private Color initialObjectColor;

    // Sound related variables
    public AudioClip buttonPressSound; // Sound clip to play when the button is pressed
    private AudioSource buttonAudioSource; // Reference to the AudioSource component

    protected override void Awake()
    {
        base.Awake();
        buttonRenderer = GetComponent<Renderer>();
        buttonRenderer.material.color = startButtonColor;

        if (objectRendererToChange != null)
        {
            objectMaterials = objectRendererToChange.materials;
            // Assuming the object has at least two materials
            initialObjectColor = objectMaterials[1].color;
        }

        // Get or add AudioSource component
        buttonAudioSource = GetComponent<AudioSource>();
        if (buttonAudioSource == null)
        {
            buttonAudioSource = gameObject.AddComponent<AudioSource>();
        }
        buttonAudioSource.playOnAwake = false;
        buttonAudioSource.clip = buttonPressSound;
    }

    private void UpdateObjectRotation()
    {
        if (isObjectRotating)
        {
            // Logic to rotate the object continuously
            rotatingObject.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactor = args.interactor;

        // Toggle the button state and color
        isButtonPressed = !isButtonPressed;
        buttonRenderer.material.color = isButtonPressed ? pressedButtonColor : startButtonColor;

        // Toggle the object rotation
        isObjectRotating = !isObjectRotating;

        // Toggle the color of the second material of the specified object
        if (objectRendererToChange != null)
        {
            Color newColor = isButtonPressed ? pressedButtonColor : initialObjectColor;
            objectMaterials[1].color = newColor;
            objectRendererToChange.materials = objectMaterials;
        }

        UpdateObjectRotation();

        // Play the button press sound
        if (buttonPressSound != null && buttonAudioSource != null)
        {
            buttonAudioSource.Play();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactor = null;

        // No need to stop the rotation here; it will be handled on the next press

        UpdateObjectRotation();
    }

    private void Update()
    {
        if (isObjectRotating)
        {
            UpdateObjectRotation();
        }
    }
}
