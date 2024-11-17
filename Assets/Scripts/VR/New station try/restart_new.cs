using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class restart_new : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downRotation = -45f; // Angle when the lever is pressed down
    public float upRotation = 0f; // Maximum angle when the lever is released

    public GameObject popUpCanvas; // Reference to the pop-up canvas

    public GameObject targetObject; // Reference to the target object
    public GameObject objectToTurnGreen; // Reference to the object you want to turn green
    private float minZRotation = -95f; // Minimum z-axis rotation of the target object for interaction
    private float maxZRotation = -85f; // Maximum z-axis rotation of the target object for interaction

    // Sound related variables
    public AudioClip leverSound; // Sound clip to play when the lever is pressed
    private AudioSource leverAudioSource; // Reference to the AudioSource component

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
    }

    private void SetLeverRotation(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.localRotation = initialRotation * targetRotation;
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);

        if (CheckCondition())
        {
            this.interactor = interactor;
            isInteracting = true;

            // Toggle the lever position
            isDown = !isDown;
            float targetRotation = isDown ? downRotation : upRotation;
            SetLeverRotation(targetRotation);

            // Display the pop-up message when the lever is pulled down
            if (isDown && popUpCanvas != null)
            {
                popUpCanvas.SetActive(true);
                StartCoroutine(HideMessage());
            }

            // Change the color of the specified material to green
            if (isDown && objectToTurnGreen != null)
            {
                Renderer renderer = objectToTurnGreen.GetComponent<Renderer>();
                if (renderer != null && renderer.materials.Length >= 2)
                {
                    // Assuming you want to change the color of the second material
                    renderer.materials[1].color = Color.green;
                }
            }

            // Play the lever sound
            if (leverSound != null && leverAudioSource != null)
            {
                leverAudioSource.Play();
            }

            // Load the new scene after 5 seconds
            StartCoroutine(LoadNewSceneAfterDelay("New VR Menu", 5f));
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }

    private bool CheckCondition()
    {
        if (targetObject != null)
        {
            // Get the z-axis rotation of the target object
            float zRotation = targetObject.transform.rotation.eulerAngles.z;

            // Normalize the rotation to a range between 0 and 360 degrees
            zRotation = NormalizeRotation(zRotation);

            // Check if the normalized zRotation is within the specified range (-85 to -95)
            if (zRotation >= NormalizeRotation(minZRotation) && zRotation <= NormalizeRotation(maxZRotation))
            {
                // The lever can be interacted with
                return true;
            }
        }

        // The lever cannot be interacted with
        return false;
    }

    private float NormalizeRotation(float rotation)
    {
        // Normalize a rotation value to be within the range of 0 to 360 degrees
        while (rotation < 0)
        {
            rotation += 360;
        }
        while (rotation >= 360)
        {
            rotation -= 360;
        }
        return rotation;
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

    IEnumerator LoadNewSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
