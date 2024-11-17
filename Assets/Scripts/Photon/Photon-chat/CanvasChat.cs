using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CanvasCaht : MonoBehaviourPunCallbacks
{
    public InputField inputField;
    public GameObject Message;
    public GameObject Content;
    // Start is called before the first frame update
    
    public void SendMessage()
    {
        GetComponent<PhotonView>().RPC("GetMessage" , RpcTarget.All, inputField.text);
    }

    [PunRPC]
    public void GetMessage(string ReceiveMessage)
    {
        Instantiate(Message, Vector3.zero, Quaternion.identity , Content.transform);
    }
}
 