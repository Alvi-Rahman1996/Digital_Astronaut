using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Security.Cryptography;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        if (PV.IsMine && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "TabletPlayerController_1"), new Vector3(-22.92f, 105.41f, -133.07f), Quaternion.identity);
            Debug.Log("Instantiated Tablet Controller");
        }
        else if (PV.IsMine && !PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "VRPlayerController"), new Vector3(-6f, 110f, -140f), Quaternion.identity);
            Debug.Log("Instantiated VR Controller");
        }
        
    }

}
