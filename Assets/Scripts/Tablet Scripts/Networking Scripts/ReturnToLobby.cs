using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.ShaderData;

public class ReturnToLobby : MonoBehaviourPunCallbacks
{

    void Start()
    {
        Invoke("Restart", 2f); 
    }

    void Restart()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        GameObject.Find("RoomManager").transform.SetParent(gameObject.transform);
        GameObject.Find("VoiceManager").transform.SetParent(gameObject.transform);

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Leaving Room as Client");
            PhotonNetwork.LeaveRoom(true);
            //Destroy(GameObject.FindGameObjectWithTag("VRPlayer"));



            PhotonNetwork.LoadLevel("New Vr Menu");
        }

    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log("Leaving Room as Master");
        PhotonNetwork.LeaveRoom(true);
        //Destroy(GameObject.FindGameObjectWithTag("TabletPlayer"));

        PhotonNetwork.LoadLevel("TabMenu");

    }


}
