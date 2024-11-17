using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases) 
    {
         _roomsCanvases = canvases;

    }

    public void OnClick_CreateRoom()
    {
        //Create Room 
        //Join Room 
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options,null );

        //PhotonNetwork.JoinOrCreateRoom(_roomName.text, options,TypedLobby.Default );
        

    }

    public override void OnCreatedRoom()
    {

        //base.OnCreatedRoom();
        Debug.Log("Created room successfully.", this);
        _roomsCanvases.CurrentRoomCanvas.Show();


    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        Debug.Log("Room Creation failed." + message, this);


    }




}
