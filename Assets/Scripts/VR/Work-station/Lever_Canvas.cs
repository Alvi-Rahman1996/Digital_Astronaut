using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class Lever_Canvas : XRBaseInteractable
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

    public Light pointLight; // Reference to the Point Light component

    public GameObject popUpCanvas; // Reference to the pop-up canvas

    public GameObject bulb; // Reference to the bulb GameObject
    private Renderer bulbRenderer; // Renderer component of the bulb

    public GameObject targetObject; // Reference to the target object
    private float minZRotation = -100f; // Minimum z-axis rotation of the target object for interaction
    private float maxZRotation = -80f; // Maximum z-axis rotation of the target object for interaction

    private PhotonView photonView; // Reference to the PhotonView component

    private Material[] bulbMaterials; // Array to store materials of the bulb
    private int secondMaterialIndex = 1; // Index of the second material

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

        // Set the initial color of the bulb to red
        bulbMaterials[secondMaterialIndex].color = Color.red;

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
                    StartCoroutine(HideMessage());
                }

                // Lever is down, change the Point Light color to green
                pointLight.color = Color.green;

                // Change the second material of the bulb to green
                if (bulbMaterials.Length > secondMaterialIndex)
                {
                    bulbMaterials[secondMaterialIndex].color = Color.green;
                }

                // Send lever state change over the network
                photonView.RPC("SyncLever4State", RpcTarget.All, isDown);
            }
            else
            {
                // Lever is up, change the Point Light color to another color if needed
                pointLight.color = Color.red; // Change to the desired color

                // Change the second material of the bulb to red or any desired color
                if (bulbMaterials.Length > secondMaterialIndex)
                {
                    bulbMaterials[secondMaterialIndex].color = Color.red;
                }

                // Send lever state change over the network
                photonView.RPC("SyncLever4State", RpcTarget.All, isDown);
            }
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

    [PunRPC]
    private void SyncLever4State(bool newIsDown)
    {
        // Synchronize the lever state across the network
        isDown = newIsDown;
        float targetRotation = isDown ? downRotation : upRotation;
        SetLeverRotation(targetRotation);

        // Display the pop-up message when the lever is pulled down
        if (isDown)
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.SetActive(true);
                StartCoroutine(HideMessage());
            }

            // Lever is down, change the Point Light color to green
            pointLight.color = Color.green;

            // Change the second material of the bulb to green
            if (bulbMaterials.Length > secondMaterialIndex)
            {
                bulbMaterials[secondMaterialIndex].color = Color.green;
            }
        }
        else
        {
            // Lever is up, change the Point Light color to another color if needed
            pointLight.color = Color.red; // Change to the desired color

            // Change the second material of the bulb to red or any desired color
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
        }
    }
}
