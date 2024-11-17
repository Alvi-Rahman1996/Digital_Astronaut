using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using UnityEngine.SceneManagement;

public class AstronautInteraction : MonoBehaviourPun
{
    private Rigidbody astronautRigidbody;
    private bool isHoldingAstronaut = false;
    private Transform robotArmEndTransform;
    private Transform astronautOriginalParent;
    private Quaternion astronautInitialRotation;

    private PhotonView photonView;
    const byte END_GAME_EVENT = 9;

    private void Awake()
    {
        // PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void Start()
    {
        astronautRigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>(); // Get the PhotonView component
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoboticArmEnd") && !isHoldingAstronaut)
        {
            robotArmEndTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RoboticArmEnd") && !isHoldingAstronaut)
        {
            robotArmEndTransform = null;
        }
    }

    private void Update()
    {
        // Check if the astronaut is not currently being held
        if (!isHoldingAstronaut)
        {
            // Check if the robotic arm end is available
            if (robotArmEndTransform != null && !PhotonNetwork.IsMasterClient)
            {
                // Pick up the astronaut
                photonView.RPC("PickUpAstronaut", RpcTarget.All);
            }
        }

        // Update the position of the astronaut while keeping the initial rotation
        if (isHoldingAstronaut)
        {
            transform.rotation = astronautInitialRotation;
        }
    }

    [PunRPC]
    private void PickUpAstronaut()
    {
        astronautOriginalParent = transform.parent;
        Debug.Log(astronautOriginalParent.name);

        // Store the initial rotation of the astronaut
        astronautInitialRotation = transform.rotation;

        // Attach the astronaut to the robotic arm end
        transform.SetParent(GameObject.FindGameObjectWithTag("RoboticArmEnd").transform);
        astronautRigidbody.isKinematic = true;

        isHoldingAstronaut = true;
    }

    [PunRPC]
    private void DetachAstronaut()
    {
        // Detach the astronaut from the robotic arm end
        transform.SetParent(astronautOriginalParent);
        astronautRigidbody.isKinematic = false;

        isHoldingAstronaut = false;
    }
}
