using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Highlight : MonoBehaviour
{
    public GameObject mainCamera;

    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    private int prevPVID = 9999;

    const byte HIGHLIGHT_EVENT = 3;
    const byte REMOVE_HIGHLIGHT_EVENT = 4;


    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            if (highlight.gameObject.GetComponent<Outline>() != null) 
                highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        //if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        if (Input.GetMouseButtonDown(0))
            {

            if (highlight)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    if (selection != null)
                    {
                        selection.gameObject.GetComponent<Outline>().enabled = false;
                    }
                    selection = raycastHit.transform;
                    selection.gameObject.GetComponent<Outline>().enabled = true;
                    highlight = null;

                    object data;

                    if (selection.gameObject.GetComponent<PhotonView>() != null)
                        data = selection.gameObject.GetComponent<PhotonView>().ViewID;
                        
                    else
                    {
                        data = null;
                    }
                    if (prevPVID != 9999)
                    {
                        object oldData;
                        oldData = prevPVID;
                        PhotonNetwork.RaiseEvent(REMOVE_HIGHLIGHT_EVENT, oldData, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.RaiseEvent(HIGHLIGHT_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                    prevPVID = selection.gameObject.GetComponent<PhotonView>().ViewID;
                }
                
            }
            else
            {
                if (selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;

                    object data;

                    if (selection.gameObject.GetComponent<PhotonView>() != null)
                        data = selection.gameObject.GetComponent<PhotonView>().ViewID;
                    else
                    {
                        data = null;
                    }


                    selection = null;


                    PhotonNetwork.RaiseEvent(REMOVE_HIGHLIGHT_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

                }
            }
        }
    }

}