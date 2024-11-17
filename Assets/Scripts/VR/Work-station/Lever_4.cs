using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Lever_4 : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downRotation = 0f; // Angle when the lever is pressed down
    public float upRotation = 45f; // Maximum angle when the lever is released

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    public Light pointLight; // Reference to the Point Light component

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
        isDown = false; // Start with the lever in the up position

        // Get or add AudioSource component
        leverAudioSource = GetComponent<AudioSource>();
        if (leverAudioSource == null)
        {
            leverAudioSource = gameObject.AddComponent<AudioSource>();
        }
        leverAudioSource.playOnAwake = false;
        leverAudioSource.clip = leverSound;
    }

    private void SetLeverRotation(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(angle, 0f, 0f);
        transform.localRotation = initialRotation * targetRotation;
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;

        // Toggle the lever position
        isDown = !isDown;
        float targetRotation = isDown ? downRotation : upRotation;
        SetLeverRotation(targetRotation);

        // Play the lever sound
        if (leverSound != null && leverAudioSource != null)
        {
            leverAudioSource.Play();
        }

        // Perform an action based on the lever's state
        if (isDown)
        {
            // Lever is down, change the Point Light color to green
            pointLight.color = Color.green;
        }
        else
        {
            // Lever is up, change the Point Light color to another color if needed
            pointLight.color = Color.white; // Change to the desired color
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }
}
