using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class MoveLever2 : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downRotation = 0f; // Angle when the lever is pressed down
    public float upRotation = 45f; // Angle when the lever is released
    public GameObject dishObject; // Reference to the Dish GameObject

    public GameObject bulb; // Reference to the bulb GameObject
    public GameObject button; // Reference to the button GameObject
    private Renderer bulbRenderer;
    private Renderer buttonRenderer; // Renderer component of the bulb
    private Material secondMaterial; // Reference to the second material of the bulb

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    private Quaternion leverInitialRotation;
    private TiltDish tiltDish; // Reference to the TiltDish script
    public Light pointLight; // Reference to the Point Light component

    private PhotonView photonView; // Reference to the PhotonView component

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

        bulbRenderer = bulb.GetComponent<Renderer>();
        buttonRenderer = button.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb and button to black
        bulbRenderer.material.color = Color.black;
        buttonRenderer.material.color = Color.black;

        // Assuming the bulb has at least two materials
        if (bulbRenderer.materials.Length >= 2)
        {
            // Assign the second material to the secondMaterial variable
            secondMaterial = bulbRenderer.materials[1];
        }

        // Get the PhotonView component
        photonView = GetComponent<PhotonView>();
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
            if (secondMaterial != null)
            {
                secondMaterial.color = Color.green;
            }
            buttonRenderer.material.color = Color.green;
        }
    }

    private void StopTilt()
    {
        if (tiltDish != null)
        {
            // Disable dish tilting
            tiltDish.StopTilt();
            if (secondMaterial != null)
            {
                secondMaterial.color = Color.red;
            }
            buttonRenderer.material.color = Color.black;
        }
    }

    [PunRPC]
    private void SyncLeverState(bool newIsDown)
    {
        isDown = newIsDown;
        SetLeverRotation();

        if (isDown)
        {
            StartTilt();
        }
        else
        {
            StopTilt();
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
