using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Local_Lever : MonoBehaviour
{
    public float leverMoveDistance = 100f; // Distance the lever moves up and down
    public float leverMoveSpeed = 10f; // Speed at which the lever moves

    private bool isLeverActivated = false;
    private Quaternion initialRotation; // Initial rotation of the lever handle

    private void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation of the lever
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
                }
            }
        }

        // Calculate the target rotation based on the activation state
        Quaternion targetRotation = isLeverActivated ? initialRotation * Quaternion.Euler(-leverMoveDistance, 0f, 0f) : initialRotation;

        // Smoothly rotate the lever towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leverMoveSpeed * Time.deltaTime);
    }
}