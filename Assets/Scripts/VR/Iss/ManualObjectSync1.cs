using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class ManualObjectSync1 : MonoBehaviourPun
{
    const byte SYNC_OBJECT_EVENT = 7;

    private void FixedUpdate()
    {
        // Create a list to store position and rotation data
        List<object> objList = new List<object>();

        // Get the position and rotation of the GameObject
        Vector3 outPos = transform.position;
        Quaternion outRot = transform.rotation;

        // Add position and rotation to the list
        objList.Add(outPos);
        objList.Add(outRot);

        // Convert the list to an array
        object data = objList.ToArray();

        // Raise a custom event to synchronize position and rotation
        PhotonNetwork.RaiseEvent(SYNC_OBJECT_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    // Callback for custom event reception
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    // Handle the received event
    private void OnEventReceived(EventData photonEvent)
    {
        if (photonEvent.Code == SYNC_OBJECT_EVENT)
        {
            // Extract position and rotation data from the received event
            object[] data = (object[])photonEvent.CustomData;

            // Make sure the data array has the expected structure
            if (data != null && data.Length == 2 && data[0] is Vector3 && data[1] is Quaternion)
            {
                // Update the position and rotation of the GameObject
                Vector3 receivedPos = (Vector3)data[0];
                Quaternion receivedRot = (Quaternion)data[1];
                transform.position = receivedPos;
                transform.rotation = receivedRot;
            }
        }
    }
}
