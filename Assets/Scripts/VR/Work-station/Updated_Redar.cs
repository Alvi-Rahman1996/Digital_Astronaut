using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updated_Redar : MonoBehaviour
{
    public float leverMoveDistance = 100f; // Distance the lever moves up and down
    public float leverMoveSpeed = 10f; // Speed at which the lever moves

    public GameObject bulb; // Reference to the bulb GameObject
    public GameObject radarDish; // Reference to the radar dish GameObject
    public AudioClip leverSound; // Lever sound
    public AudioClip loopingSound; // Looping sound

    private bool isLeverActivated = false;
    private Quaternion initialRotation; // Initial rotation of the lever handle
    private Renderer bulbRenderer; // Renderer component of the bulb
    private Quaternion initialRadarRotation; // Initial rotation of the radar dish
    private Quaternion previousRadarRotation; // Rotation of the radar dish when the lever was last deactivated

    private AudioSource audioSource;
    private bool isLoopingSoundPlaying = false;

    private void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation of the lever
        bulbRenderer = bulb.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb to red
        bulbRenderer.material.color = Color.red;

        initialRadarRotation = radarDish.transform.rotation; // Store the initial rotation of the radar dish
        previousRadarRotation = initialRadarRotation; // Set the previous radar rotation to initial rotation

        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
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
                        audioSource.PlayOneShot(leverSound); // Play lever sound

                        // Start playing the looping sound
                        if (!isLoopingSoundPlaying)
                        {
                            audioSource.clip = loopingSound;
                            audioSource.loop = true;
                            audioSource.Play();
                            isLoopingSoundPlaying = true;
                        }
                    }
                    else
                    {
                        // Perform actions when the lever is deactivated
                        bulbRenderer.material.color = Color.red; // Change the bulb color to red
                        previousRadarRotation = radarDish.transform.rotation; // Store the current radar rotation

                        // Stop playing the looping sound
                        if (isLoopingSoundPlaying)
                        {
                            audioSource.Stop();
                            audioSource.loop = false;
                            isLoopingSoundPlaying = false;
                        }

                        audioSource.PlayOneShot(leverSound); // Play lever sound
                    }
                }
            }
        }

        Quaternion targetRotation = isLeverActivated ? initialRotation * Quaternion.Euler(-leverMoveDistance, 0f, 0f) : initialRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leverMoveSpeed * Time.deltaTime);

        if (isLeverActivated)
        {
            // Rotate the radar dish
            radarDish.transform.rotation = Quaternion.Euler(0f, Mathf.Sin(Time.time) * 30f, 0f) * initialRadarRotation;
        }
        else
        {
            // Stop the rotation of the radar dish and resume from previous rotation
            radarDish.transform.rotation = previousRadarRotation;
        }
    }
}