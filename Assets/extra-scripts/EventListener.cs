using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventListener : MonoBehaviour
{
    [SerializeField] GameObject restartGO;

    bool isWorking = false;

    LineRenderer lineRenderer;
    const byte DRAW_EVENT = 0;
    const byte LASER_EVENT = 1;
    const byte CLEAR_LINE_EVENT = 2;
    const byte HIGHLIGHT_EVENT = 3;
    const byte REMOVE_HIGHLIGHT_EVENT = 4;
    const byte END_GAME_EVENT = 9;


    PhotonView PV;
    // Start is called before the first frame update

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        lineRenderer = GetComponent<LineRenderer>();
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    public void ManualRestart()
    {
        PV.RPC("ManualRestartRPC", RpcTarget.All);
    }

    [PunRPC]
    void ManualRestartRPC()
    {
        restartGO.SetActive(true);
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        //Debug.Log("Event Received: " + obj.Code);
        if (obj.Code == DRAW_EVENT)
        {

            if (isWorking)
            {
                return;
            }
            else
            {
                isWorking = true;

                Vector3 vect;
                List<Vector3> vectList = new List<Vector3>();

                object[] data = (object[])obj.CustomData;
                if (data.Length > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        vect = (Vector3)data[i];
                        vectList.Add(vect);
                    }
                }
                //Debug.Log(data.Length);
                //Debug.Log(vectList.Count);
                //Debug.Log(vectList);

                if (vectList.Count > 0)
                {
                    //Debug.Log("We have stuff! " + vectList[0]);
                }

                lineRenderer.positionCount = vectList.Count;
                lineRenderer.SetPositions(vectList.ToArray());
                isWorking = false; 

                //Debug.Log(data[0]);

                /*
                Vector3[] VectData= new Vector3[] { };

                Debug.Log(data[0]);

                for (int i = 0; i < data.Length/3; i += 3)
                {
                    x = (float)data[i];
                    y = (float)data[i+1];
                    z = (float)data[i+2];
                    VectData.Append(new Vector3(x, y, z));
                }

                Debug.Log("Incoming Data: " + VectData[0]);
                Vector3[] linePosNet = data.Cast<Vector3>().ToArray();
                Debug.Log(linePosNet);
                Debug.Log("Linepos length = " + linePosNet.Length);
                */
            }
        }

        if (obj.Code == LASER_EVENT)
        {
            if (isWorking)
            {
                return;
            }
            else
            {
                isWorking = true;

                Vector3 vect;
                List<Vector3> vectList = new List<Vector3>();

                object[] data = (object[])obj.CustomData;
                if (data.Length > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        vect = (Vector3)data[i];
                        vectList.Add(vect);
                    }

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, vectList[0]);
                    lineRenderer.SetPosition(1, vectList[1]);
                    isWorking = false;
                }
            }
        }

        if (obj.Code == CLEAR_LINE_EVENT)
        {
            if (isWorking)
            {
                return;
            }
            else
            {
                isWorking = true;
                if (lineRenderer != null)
                    lineRenderer.positionCount = 0;

                isWorking = false;
            }
        }

        if (obj.Code == HIGHLIGHT_EVENT)
        {
            if (isWorking)
            {
                return;
            }
            else
            {
                isWorking = true;
                int viewID = 9999;
                viewID = (int)obj.CustomData;

                PhotonView selection = PhotonView.Find(viewID);



                if (selection.gameObject.GetComponent<Outline>() == null)
                {
                    Outline outline = selection.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    selection.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    selection.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
                else
                {
                    selection.gameObject.GetComponent<Outline>().enabled = true;
                }

                isWorking = false;
            }
        }

        if (obj.Code == REMOVE_HIGHLIGHT_EVENT)
        {
            if (isWorking)
            {
                return;
            }
            else
            {
                isWorking = true;

                int viewID = 9999;
                viewID = (int)obj.CustomData;

                PhotonView selection = PhotonView.Find(viewID);

                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }

                isWorking = false;
            }
        }

        if (obj.Code == END_GAME_EVENT)
        {
            PhotonNetwork.AutomaticallySyncScene = false;

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Leaving Room as Master");
                PhotonNetwork.LeaveRoom(true);
                SceneManager.LoadScene("Lobby 1");
            }
            Debug.Log("Leaving Room as Client");
            PhotonNetwork.LeaveRoom(true);
            SceneManager.LoadScene("Rooms 1");


        }

        void TabletPlayerLeave()
        {
            //Debug.Log("TabletPlayerLeave Invoked");

            Invoke("TabletPlayerLeave", 3);
        }

    }
}
