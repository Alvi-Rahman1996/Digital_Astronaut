using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updated_Main_Radar : MonoBehaviour
{
    public float leverMoveDistance = 100f; // Distance the lever moves up and down
    public float leverMoveSpeed = 10f; // Speed at which the lever moves
    public float tiltAngle = 0.1f; // Angle at which the radar dish tilts on the specified axis
    public float tiltSpeed = 1f; // Speed at which the radar dish tilts

    public GameObject bulb; // Reference to the bulb GameObject
    public GameObject radarDish; // Reference to the radar dish GameObject
    public AudioClip leverSound; // Lever sound
    public AudioClip loopingSound; // Looping sound

    private bool isLeverActivated = false;
    private Quaternion initialRotation; // Initial rotation of the lever handle
    private Renderer bulbRenderer; // Renderer component of the bulb
    private Quaternion initialRadarRotation; // Initial rotation of the radar dish
    private Quaternion previousRadarRotation; // Rotation of the radar dish when the lever was last deactivated

    private AudioSource leverAudioSource; // AudioSource component for lever sounds
    private AudioSource loopAudioSource; // AudioSource component for loop sound
    private bool isLoopingSoundPlaying = false;

    private void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation of the lever
        bulbRenderer = bulb.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb to red
        bulbRenderer.material.color = Color.red;

        initialRadarRotation = radarDish.transform.rotation; // Store the initial rotation of the radar dish
        previousRadarRotation = initialRadarRotation; // Set the previous radar rotation to initial rotation

        // Get the existing AudioSource components
        leverAudioSource = GetComponent<AudioSource>();
        loopAudioSource = GetComponent<AudioSource>();

        leverAudioSource.clip = leverSound;
        loopAudioSource.clip = loopingSound;
        loopAudioSource.loop = true;
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
                        leverAudioSource.Play(); // Play the lever sound

                        // Start playing the loop sound
                        if (!isLoopingSoundPlaying)
                        {
                            loopAudioSource.Play(); // Play the loop sound
                            isLoopingSoundPlaying = true;
                        }
                    }
                    else
                    {
                        // Perform actions when the lever is deactivated
                        bulbRenderer.material.color = Color.red; // Change the bulb color to red
                        previousRadarRotation = radarDish.transform.rotation; // Store the current radar rotation

                        // Stop playing the loop sound
                        if (isLoopingSoundPlaying)
                        {
                            loopAudioSource.Stop(); // Stop the loop sound
                            isLoopingSoundPlaying = false;
                        }

                        leverAudioSource.Play(); // Play the lever sound
                    }
                }
            }
        }

        Quaternion targetRotation = isLeverActivated ? initialRotation * Quaternion.Euler(-leverMoveDistance, 0f, 0f) : initialRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leverMoveSpeed * Time.deltaTime);

        if (isLeverActivated)
        {
            // Tilt the radar dish like a seesaw on the right and left axis
            float tiltFactor = Mathf.Sin(Time.time * tiltSpeed) * tiltAngle;
            Quaternion targetRadarRotation = initialRadarRotation * Quaternion.Euler(0f, tiltFactor, 0f);
            radarDish.transform.rotation = targetRadarRotation;
        }
        else
        {
            // Stop the tilt of the radar dish and resume from previous rotation
            radarDish.transform.rotation = previousRadarRotation;
        }
    }
}
