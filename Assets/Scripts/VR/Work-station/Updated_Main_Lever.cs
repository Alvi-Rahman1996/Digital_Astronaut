using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updated_Main_Lever : MonoBehaviour

{
    public float leverMoveDistance = 100f; // Distance the lever moves up and down
    public float leverMoveSpeed = 10f; // Speed at which the lever moves

    public GameObject bulb; // Reference to the bulb GameObject
    public AudioClip leverOnSound; // Sound clip for when the lever is turned on
    public AudioClip leverOffSound; // Sound clip for when the lever is turned off

    private bool isLeverActivated = false;
    private Quaternion initialRotation; // Initial rotation of the lever handle
    private Renderer bulbRenderer; // Renderer component of the bulb
    private AudioSource audioSource; // Audio source component to play the sounds

    private void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation of the lever

        bulbRenderer = bulb.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb to red
        bulbRenderer.material.color = Color.red;

        // Get the AudioSource component attached to the same GameObject or its parent
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            //Debug.LogError("AudioSource component not found! Make sure it's attached to the same GameObject or its parent.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isLeverActivated = !isLeverActivated; // Toggle the activation state

                    if (isLeverActivated)
                    {
                        // Perform actions when the lever is activated
                        bulbRenderer.material.color = Color.green; // Change the bulb color to green

                        // Play the lever on sound
                        if (leverOnSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(leverOnSound);
                        }
                    }
                    else
                    {
                        // Perform actions when the lever is deactivated
                        bulbRenderer.material.color = Color.red; // Change the bulb color to red

                        // Play the lever off sound
                        if (leverOffSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(leverOffSound);
                        }
                    }
                }
            }
        }

        Quaternion targetRotation = isLeverActivated ? initialRotation * Quaternion.Euler(-leverMoveDistance, 0f, 0f) : initialRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leverMoveSpeed * Time.deltaTime);
    }
}