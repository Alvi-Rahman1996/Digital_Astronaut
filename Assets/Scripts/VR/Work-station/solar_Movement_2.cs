using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class solar_Movement_2 : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downRotation = 0f; // Angle when the lever is pressed down
    public float upRotation = 45f; // Angle when the lever is released
    public GameObject dishObject; // Reference to the Dish GameObject


    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    private Quaternion leverInitialRotation;
    private RotateDish rotateDish; // Reference to the RotateDish script


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
            // Get the RotateDish component
            rotateDish = dishObject.GetComponent<RotateDish>();
        }
    }

    private void SetLeverRotation()
    {
        Quaternion targetRotation = isDown ? Quaternion.Euler(downRotation, 0f, 0f) : Quaternion.Euler(upRotation, 0f, 0f);
        transform.localRotation = leverInitialRotation * targetRotation;
    }

    private void StartRotation()
    {
        if (rotateDish != null)
        {
            // Enable dish rotation
            rotateDish.Rotate();
        }
    }

    private void StopRotation()
    {
        if (rotateDish != null)
        {
            // Disable dish rotation
            rotateDish.StopRotation();
        }
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;

        // Toggle the lever position
        isDown = !isDown;
        SetLeverRotation();

        // Play the lever sound
        if (leverSound != null && leverAudioSource != null)
        {
            leverAudioSource.Play();
        }

        // Perform an action based on the lever's state
        if (isDown)
        {
            // Lever is down, start rotating the dish
            StartRotation();
        }
        else
        {
            // Lever is up, stop rotating the dish
            StopRotation();
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }



}