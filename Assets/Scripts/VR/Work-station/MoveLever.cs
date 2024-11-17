using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class MoveLever : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downRotation = 0f; // Angle when the lever is pressed down
    public float upRotation = 1f; // Angle when the lever is released
    public GameObject dishObject; // Reference to the Dish GameObject

    public GameObject bulb; // Reference to the bulb GameObject
    public GameObject button; // Reference to the bulb GameObject
    private Renderer bulbRenderer;
    private Renderer buttonRenderer; // Renderer component of the bulb
    private bool isLeverActivated = false;

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    private Quaternion leverInitialRotation;
    private RotateDish rotateDish; // Reference to the RotateDish script

    private PhotonView photonView; // Reference to the PhotonView component
    private Material secondMaterial; // Reference to the second material of the bulb

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

        // Get the PhotonView component
        photonView = GetComponent<PhotonView>();

        bulbRenderer = bulb.GetComponent<Renderer>();
        buttonRenderer = button.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb to red
        bulbRenderer.material.color = Color.black;
        buttonRenderer.material.color = Color.black;

        // Assuming the bulb has at least two materials
        if (bulbRenderer.materials.Length >= 2)
        {
            // Assign the second material to the secondMaterial variable
            secondMaterial = bulbRenderer.materials[1];
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

    [PunRPC]
    private void SyncLeverState(bool newIsDown)
    {
        isDown = newIsDown;
        SetLeverRotation();

        if (isDown)
        {
            StartRotation();
            if (secondMaterial != null)
            {
                secondMaterial.color = Color.green;
            }
            buttonRenderer.material.color = Color.green;
        }
        else
        {
            StopRotation();
            // Change the second material to red
            if (secondMaterial != null)
            {
                secondMaterial.color = Color.red;
            }
            buttonRenderer.material.color = Color.black;
        }

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
        SetLeverRotation();

        // Play the lever sound
        if (leverSound != null && leverAudioSource != null)
        {
            leverAudioSource.Play();
        }

        // Call an RPC to synchronize lever state across the network
        photonView.RPC("SyncLeverState", RpcTarget.All, isDown);
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }
}
