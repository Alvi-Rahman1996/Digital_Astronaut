using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Runtime.ExceptionServices;
using System.Data.SqlTypes;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    private void Awake()
    {
        Instance = this; 
    }

    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        //Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();        
    }


    public override void OnJoinedLobby()
    {
        //Debug.Log("In launcher opening temp");
        PhotonNetwork.AutomaticallySyncScene = false;
        MenuManager.instance.OpenMenu("temp");
        //Debug.Log("Joined lobby");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInput.text);
        MenuManager.instance.OpenMenu("loading");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("temp");

    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        MenuManager.instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        //Debug.Log("Joined lobby: " + roomNameText.text);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        bool isAlreadyInList = false;
        Debug.Log("Current Room list = "+roomList.Count);

        //Debug.Log("Room List Item = "+roomListItemPrefab);
        //Debug.Log("Room List Content = "+ roomListContent);
        foreach (RoomInfo room  in roomList)
        {

            if (room.RemovedFromList)
            {
                foreach (Transform trans in roomListContent)
                {
                    if (trans.gameObject.GetComponent<RoomListItem>().text.text == room.Name)
                    {
                        Destroy(trans.gameObject);
                    }
                }
            }
            else
            {
                foreach (Transform trans in roomListContent)
                {
                    if (trans.gameObject.GetComponent<RoomListItem>().text.text == room.Name)
                    {
                        isAlreadyInList = true;
                    }
                }
                if(!isAlreadyInList)
                {
                    Debug.Log("Creating Room with Name = " + room.Name);
                    Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
                }
            }
        }
        {
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(1);
        }

    }
}
