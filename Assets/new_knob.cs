using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class new_knob : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isButtonDown; // Track whether the lever is currently pressed down or not

    public float downRotation = 10f; // Angle when the lever is pressed down
    public float upRotation = 0f; // Angle when the lever is released
    public GameObject dishObject; // Reference to the Dish GameObject

    private AudioSource leverAudioSource; // Reference to the AudioSource component

    private PhotonView photonView; // Reference to the PhotonView component

    private RotateDish rotateDish; // Reference to the RotateDish script

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
        isButtonDown = false; // Start with the lever in the up position
        SetLeverRotation();

        // Get or add AudioSource component
        leverAudioSource = GetComponent<AudioSource>();
        if (leverAudioSource == null)
        {
            leverAudioSource = gameObject.AddComponent<AudioSource>();
        }
        leverAudioSource.playOnAwake = false;

        // Get the PhotonView component
        photonView = GetComponent<PhotonView>();

        if (dishObject != null)
        {
            // Get the RotateDish component
            rotateDish = dishObject.GetComponent<RotateDish>();
        }
    }

    private void SetLeverRotation()
    {
        Quaternion targetRotation = isButtonDown ? Quaternion.Euler(downRotation, 0f, 0f) : Quaternion.Euler(upRotation, 0f, 0f);
        transform.localRotation = initialRotation * targetRotation;
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

    [PunRPC]
    private void SyncLeverState(bool newIsButtonDown)
    {
        isButtonDown = newIsButtonDown;
        SetLeverRotation();

        if (isButtonDown)
        {
            // Start dish rotation when the lever is pressed down
            StartRotation();
        }
        else
        {
            // Stop dish rotation when the lever is released
            StopRotation();
        }

        if (leverAudioSource != null)
        {
            leverAudioSource.Play();
        }
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;

        // Toggle the button state
        isButtonDown = true;
        SetLeverRotation();

        // Play the lever sound
        if (leverAudioSource != null)
        {
            leverAudioSource.Play();
        }

        // Start dish rotation when the lever is pressed down
        StartRotation();

        // Call an RPC to synchronize lever state across the network
        photonView.RPC("SyncLeverState", RpcTarget.All, isButtonDown);
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;

        // Toggle the button state
        isButtonDown = false;
        SetLeverRotation();

        // Stop dish rotation when the lever is released
        StopRotation();

        // Call an RPC to synchronize lever state across the network
        photonView.RPC("SyncLeverState", RpcTarget.All, isButtonDown);
    }
}