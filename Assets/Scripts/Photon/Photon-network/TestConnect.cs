using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestConnect : MonoBehaviourPunCallbacks
{

    
    private void Start()
    {

        
        PhotonNetwork.AutomaticallySyncScene =true;
        print("Connecting to server");
        PhotonNetwork.GameVersion ="0.0.1";
        //PhotonNetwork.NickName = MasterManager.GameSettings.NickName;

        //PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
       
        
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to server");
        //print(PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason" + cause.ToString());

        //base.OnDisconnected(cause);
    }
    
}
