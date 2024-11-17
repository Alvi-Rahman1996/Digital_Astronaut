using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class dishrot2 : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not
    private bool isDishMoving; // Track whether the dish is currently moving
    private bool isButtonPressed; // Track whether the button is currently pressed

    public float downRotation = 0f; // Angle when the lever is pressed down
    public float upRotation = 45f; // Angle when the lever is released
    public GameObject dishObject; // Reference to the Dish GameObject

    public GameObject button; // Reference to the button GameObject
    private Renderer buttonRenderer; // Renderer component of the button

    public GameObject bulb; // Reference to the bulb GameObject
    private Renderer bulbRenderer; // Renderer component of the bulb

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    private Quaternion leverInitialRotation;
    private TiltDish tiltDish; // Reference to the TiltDish script
    public Light pointLight; // Reference to the Point Light component

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
        leverInitialRotation = initialRotation;
        isDown = false; // Start with the lever in the up position
        SetLeverRotation();

        // Get or add AudioSource component
        leverAudioSource = GetComponent<AudioSource>();
        if (leverAudioSource == null)
        {
            leverAudioSource = gameObject.AddComponent<AudioSource>();
        }
        leverAudioSource.playOnAwake = false;
        leverAudioSource.clip = leverSound;

        if (dishObject != null)
        {
            // Get the TiltDish component
            tiltDish = dishObject.GetComponent<TiltDish>();
        }

        bulbRenderer = bulb.GetComponent<Renderer>(); // Get the Renderer component of the bulb
        bulbRenderer.material.color = Color.red;

        buttonRenderer = button.GetComponent<Renderer>(); // Get the Renderer component of the button
    }

    private void SetLeverRotation()
    {
        Quaternion targetRotation = isDown ? Quaternion.Euler(downRotation, 0f, 0f) : Quaternion.Euler(upRotation, 0f, 0f);
        transform.localRotation = leverInitialRotation * targetRotation;
    }

    private void StartTilt()
    {
        if (tiltDish != null)
        {
            // Enable dish tilting
            tiltDish.StartTilt();

            // Toggle the color of the second material of the specified object
            if (bulbRenderer != null && bulbRenderer.materials.Length > 1)
            {
                Color newColor = isButtonPressed ? Color.red : Color.green;
                bulbRenderer.materials[1].color = newColor;
            }

            isDishMoving = true;
        }
    }

    private void StopTilt()
    {
        if (tiltDish != null)
        {
            // Disable dish tilting
            tiltDish.StopTilt();

            // Toggle the color of the second material of the specified object
            if (bulbRenderer != null && bulbRenderer.materials.Length > 1)
            {
                Color newColor = isButtonPressed ? Color.red : Color.green;
                bulbRenderer.materials[1].color = newColor;
            }

            isDishMoving = false;
        }
    }

    private void SyncLeverState()
    {
        // Synchronize the lever state
        SetLeverRotation();

        // Perform an action based on the lever's state
        if (isDown)
        {
            // Lever is down, start tilting the dish
            StartTilt();
            // Lever is down, change the Point Light color to green
            pointLight.color = Color.green;
        }
        else
        {
            // Lever is up, stop tilting the dish
            StopTilt();
            pointLight.color = Color.red; // Change to the desired color
        }

        // Play the lever sound
        if (leverSound != null && leverAudioSource != null)
        {
            leverAudioSource.Play();
        }
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;

        // Toggle the lever position
        isDown = !isDown;
        SyncLeverState();

        // Play the lever sound
        if (leverSound != null && leverAudioSource != null)
        {
            leverAudioSource.Play();
        }

        // Toggle the button state
        isButtonPressed = !isButtonPressed;

        // Change the color of the button to green
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = isButtonPressed ? Color.green : Color.red;
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;

        // Toggle the color of the button back to its original color
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = isButtonPressed ? Color.green : Color.black;
        }
    }
}
