using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverForSolar : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialLeverRotation;
    private Quaternion initialDishRotation;
    private bool isDown;

    public float downRotation = 0f;
    public float upRotation = -45f;
    public GameObject dishObject;

    public AudioClip leverSound;
    private AudioSource leverAudioSource;

    private RotateDish rotateDish;

    protected override void Awake()
    {
        base.Awake();
        initialLeverRotation = transform.localRotation;
        isDown = false;
        SetLeverRotation();

        leverAudioSource = GetComponent<AudioSource>();
        if (leverAudioSource == null)
        {
            leverAudioSource = gameObject.AddComponent<AudioSource>();
        }
        leverAudioSource.playOnAwake = false;
        leverAudioSource.clip = leverSound;

        if (dishObject != null)
        {
            initialDishRotation = dishObject.transform.localRotation;
            rotateDish = dishObject.GetComponent<RotateDish>();
        }
    }

    private void SetLeverRotation()
    {
        Quaternion targetRotation = isDown ? Quaternion.Euler(downRotation, 0f, 0f) : Quaternion.Euler(upRotation, 0f, 0f);
        transform.localRotation = initialLeverRotation * targetRotation;
    }

    private void StartRotation()
    {
        if (rotateDish != null)
        {
            // Rotate the dish by a specific angle
            rotateDish.Rotate(); // Start the rotation
            Invoke("StopRotation", 1.0f); // Stop the rotation after a delay (adjust as needed)
        }
    }

    private void StopRotation()
    {
        if (rotateDish != null)
        {
            // Stop dish rotation
            rotateDish.StopRotation();
        }
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;

        isDown = !isDown;
        SetLeverRotation();

        if (leverSound != null && leverAudioSource != null)
        {
            leverAudioSource.Play();
        }

        if (isDown)
        {
            StartRotation();
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }
}
