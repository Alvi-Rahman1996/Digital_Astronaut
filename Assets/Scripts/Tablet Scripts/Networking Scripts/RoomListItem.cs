using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text text;
    RoomInfo info;
    public void SetUp(RoomInfo _info)
        {
            info = _info;
            text.text = _info.Name;
        }

    public void OnClick()
        {
            //Debug.Log("clicking on room with name: "+info.Name);
            Launcher.Instance.JoinRoom(info);
        }
}
