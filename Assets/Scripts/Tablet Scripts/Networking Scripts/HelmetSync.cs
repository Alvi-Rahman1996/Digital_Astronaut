using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetSync : MonoBehaviour
{
    const byte SYNC_HMD_EVENT = 5;
    bool isWorking = false;
    bool isListening = true;

    private void Awake()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    public void TrackHMD()
    {
        if (!isListening) 
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
            isListening = true;
        }
    }

    public void UntrackHMD()
    {
        if(isListening)
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
            isListening = false;

        }

    }
    // Update is called once per frame
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if(obj.Code == SYNC_HMD_EVENT)
        {
            if (isWorking)
            {
                return;
            }
            else
            {
                isWorking = true;


                Vector3 inPos;
                Quaternion inRot;

                object[] data = (object[])obj.CustomData;


                inPos = (Vector3)data[0]; 
                inRot = (Quaternion)data[1];    

                if (gameObject != null) 
                    gameObject.transform.SetPositionAndRotation(inPos, inRot) ;

                isWorking = false;
            }
        }
    }

    }
