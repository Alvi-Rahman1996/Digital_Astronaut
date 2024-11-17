using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updated_Local_Lever : MonoBehaviour

{
    public float leverMoveDistance = 100f; // Distance the lever moves up and down
    public float leverMoveSpeed = 10f; // Speed at which the lever moves

    public AudioClip clickSound; // Sound clip for the lever click

    private bool isLeverActivated = false;
    private Quaternion initialRotation; // Initial rotation of the lever handle
    private AudioSource audioSource; // Audio source component to play the sound

    private void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation of the lever

        // Get the AudioSource component attached to the same GameObject or its parent
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            //Debug.LogError("AudioSource component not found! Make sure it's attached to the same GameObject or its parent.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !(PhotonNetwork.IsMasterClient))
        {
            // Check if the mouse click hits the lever collider
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isLeverActivated = !isLeverActivated; // Toggle the activation state

                    // Play the click sound
                    if (clickSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(clickSound);
                    }
                }
            }
        }

        // Calculate the target rotation based on the activation state
        Quaternion targetRotation = isLeverActivated ? initialRotation * Quaternion.Euler(-leverMoveDistance, 0f, 0f) : initialRotation;

        // Smoothly rotate the lever towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leverMoveSpeed * Time.deltaTime);
    }
}