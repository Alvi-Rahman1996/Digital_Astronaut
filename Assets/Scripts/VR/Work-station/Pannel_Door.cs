using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun; // Add Photon.Pun namespace

public class Pannel_Door : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the door is currently open or not

    public float downRotation = 120f; // Angle when the door is open
    public float upRotation = 0f; // Maximum angle when the door is closed

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the door is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    

        private PhotonView photonView; // Reference to the PhotonView component

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


        photonView = GetComponent<PhotonView>(); // Get the PhotonView component
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

        
       
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }

    [PunRPC]
    private void SyncLever4State(bool newIsDown)
    {
        // Synchronize the lever state across the network
        isDown = newIsDown;
        float targetRotation = isDown ? downRotation : upRotation;
        SetLeverRotation(targetRotation);

    }

   
}

