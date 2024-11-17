using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;




public class Popup_Window_2 : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downRotation = -45f; // Angle when the lever is pressed down
    public float upRotation = 0f; // Maximum angle when the lever is released

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

    

    public GameObject popUpCanvas; // Reference to the pop-up canvas
    public GameObject restartGO; //Script that restarts the game to lobby

    public GameObject bulb; // Reference to the bulb GameObject
    private Renderer bulbRenderer; // Renderer component of the bulb

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

        bulbRenderer = bulb.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb to red
        bulbRenderer.material.color = Color.red;
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

        // Display the pop-up message when the lever is pulled down
        if (isDown)
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.SetActive(true);
                StartCoroutine(HideMessage());
            }

           
            bulbRenderer.material.color = Color.green;
        }
        else
        {
           
            bulbRenderer.material.color = Color.red;
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(8f); // Adjust the duration as needed

        // Disable the pop-up canvas
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(false);
        }
    }
}
