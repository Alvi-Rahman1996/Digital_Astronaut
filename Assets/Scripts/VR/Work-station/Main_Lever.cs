using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Main_Lever : MonoBehaviour
{
    public float leverMoveDistance = 100f; // Distance the lever moves up and down
    public float leverMoveSpeed = 10f; // Speed at which the lever moves

    public GameObject bulb; // Reference to the bulb GameObject

    private bool isLeverActivated = false;
    private Quaternion initialRotation; // Initial rotation of the lever handle
    private Renderer bulbRenderer; // Renderer component of the bulb

    private void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation of the lever

        bulbRenderer = bulb.GetComponent<Renderer>(); // Get the Renderer component of the bulb

        // Set the initial color of the bulb to red
        bulbRenderer.material.color = Color.red;
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
                    }
                    else
                    {
                        // Perform actions when the lever is deactivated
                        bulbRenderer.material.color = Color.red; // Change the bulb color to red
                    }
                }
            }
        }

        Quaternion targetRotation = isLeverActivated ? initialRotation * Quaternion.Euler(-leverMoveDistance, 0f, 0f) : initialRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, leverMoveSpeed * Time.deltaTime);
    }
}










