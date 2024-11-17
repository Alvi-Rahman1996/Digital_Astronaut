using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class Restart_script : XRBaseInteractable
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
    public GameObject restartGO; // Script that restarts the game to lobby

    public GameObject bulb; // Reference to the bulb GameObject
    private Renderer bulbRenderer; // Renderer component of the bulb
    private Material[] bulbMaterials; // Array to store materials of the bulb
    private int secondMaterialIndex = 1; // Index of the second material

    private bool isCanvasVisible = false;

    public GameObject targetObject; // Reference to the target object
    public float minZRotation = -95f; // Minimum z-axis rotation for interaction
    public float maxZRotation = -85f; // Maximum z-axis rotation for interaction

    private PhotonView photonView; // Reference to the PhotonView component
    const byte END_GAME_EVENT = 9;

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
        bulbMaterials = bulbRenderer.materials; // Get all materials of the bulb

        // Set the initial color of the second material of the bulb to red
        if (bulbMaterials.Length > secondMaterialIndex)
        {
            bulbMaterials[secondMaterialIndex].color = Color.red;
        }

        photonView = GetComponent<PhotonView>(); // Get the PhotonView component
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
                    isCanvasVisible = true;
                    StartCoroutine(HideMessage());
                    // Change the color of the second material of the bulb to green
                    if (bulbMaterials.Length > secondMaterialIndex)
                    {
                        bulbMaterials[secondMaterialIndex].color = Color.green;
                    }
                }

                // Send lever state change over the network
                photonView.RPC("SyncLeverState4", RpcTarget.All, isDown);
            }
            else
            {
                // Change the color of the second material of the bulb to red
                if (bulbMaterials.Length > secondMaterialIndex)
                {
                    bulbMaterials[secondMaterialIndex].color = Color.red;
                }

                // Send lever state change over the network
                photonView.RPC("SyncLeverState4", RpcTarget.All, isDown);
            }
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }

    [PunRPC]
    private void SyncLeverState4(bool newIsDown)
    {
        // Synchronize the lever state across the network
        isDown = !isDown;
        float targetRotation = isDown ? downRotation : upRotation;
        SetLeverRotation(targetRotation);

        // Display the pop-up message when the lever is pulled down
        if (isDown)
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.SetActive(true);
                isCanvasVisible = true;
                StartCoroutine(HideMessage());
            }

            // Change the color of the second material of the bulb to green
            if (bulbMaterials.Length > secondMaterialIndex)
            {
                bulbMaterials[secondMaterialIndex].color = Color.green;
            }
        }
        else
        {
            // Change the color of the second material of the bulb to red
            if (bulbMaterials.Length > secondMaterialIndex)
            {
                bulbMaterials[secondMaterialIndex].color = Color.red;
            }
        }
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(8f); // Adjust the duration as needed

        // Disable the pop-up canvas
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(false);
            isCanvasVisible = false;

            // Add code to restart the game here
            RestartGameAfterDelay(5f); // Restart the game after a 5-second delay
        }
    }

    private void RestartGameAfterDelay(float delay)
    {
        StartCoroutine(RestartGameCoroutine(delay));
    }

    private IEnumerator RestartGameCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        restartGO.SetActive(true);

        // Reset any necessary variables or game state here

        // Reload the current scene to restart the game
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        //object data= null;
        // PhotonNetwork.RaiseEvent(END_GAME_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    private bool CheckCondition()
    {
        if (targetObject != null)
        {
            // Get the z-axis rotation of the target object
            float zRotation = targetObject.transform.rotation.eulerAngles.z;

            // Normalize the rotation to a range between 0 and 360 degrees
            zRotation = NormalizeRotation(zRotation);

            // Check if the normalized zRotation is within the specified range
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
}
