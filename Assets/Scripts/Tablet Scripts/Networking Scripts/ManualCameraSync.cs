using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCameraSync : MonoBehaviour
{
    const byte SYNC_HMD_EVENT = 5;
    private void FixedUpdate()
    {
        List<object> objList = new List<object>();


        Vector3 outPos;
        Quaternion outRot;
        gameObject.transform.GetPositionAndRotation(out outPos, out outRot);

        objList.Add(outPos);
        objList.Add(outRot);
        object data = objList.ToArray();
        PhotonNetwork.RaiseEvent(SYNC_HMD_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}
