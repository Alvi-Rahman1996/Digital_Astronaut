using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletUIManager : MonoBehaviour

{
    public Button laserButton;
    public Button drawButton;
    public Button highlightButton;

    [SerializeField] Sprite laserButtonImageSelected, laserButtonImageUnselected, drawingButtonImageSelected, drawingButtonImageUnselected, highlightButtonImageSelected, highlightButtonImageUnselected;

    public GameObject laserObject;

    private string globalCurrentCamera;

    private Boolean laserEnabled = false;
    private Boolean drawingEnabled = false;
    private Boolean highlightEnabled = false;

    private object[] data;
    const byte CLEAR_LINES_EVENT = 2;

    void Start()
    {
        DisableAll();
    }

    public void DisableAll()
    {
        DisableHighlight();
        DisableDrawing();
        DisableLaser();

        laserButton.interactable = false;
        drawButton.interactable = false;
        highlightButton.interactable = false;
    }

    public void EnableAll()
    {
        laserButton.interactable = true;
        drawButton.interactable = true;
        highlightButton.interactable = true;
    }


    public void OnClick_HighlightButton()
    {

        DisableLaser();
        DisableDrawing();

        highlightEnabled = !highlightEnabled;
        highlightButton.GetComponent<Highlight>().enabled = highlightEnabled;
        highlightButton.GetComponent<Image>().sprite = highlightButtonImageSelected;

        if (!highlightEnabled)
        {
            DisableHighlight();
        }
    }

    void DisableHighlight()
    {
        foreach (GameObject selectableObject in GameObject.FindGameObjectsWithTag("Selectable"))
        {
            if (selectableObject.GetComponent<Outline>() != null)
                selectableObject.GetComponent<Outline>().enabled = false;
        }
        highlightButton.GetComponent<Image>().sprite = highlightButtonImageUnselected;
    }
    public void OnClick_LaserButton()
    {
        if (!laserEnabled)
        {
            laserObject.SetActive(true);
            ColorBlock colors = laserButton.colors;
            colors.normalColor = Color.gray;
            colors.selectedColor = Color.gray;
            // laserButton.colors = colors;
            laserEnabled = true;
            laserButton.GetComponent<Image>().sprite = laserButtonImageSelected;


        }
        else
        {
            DisableLaser();
        }
        DisableDrawing();
        DisableHighlight();
    }

    void DisableLaser()
    {
        laserObject.SetActive(false);
        ColorBlock colors = laserButton.colors;
        colors.normalColor = Color.white;
        colors.selectedColor = Color.white;
        // laserButton.colors = colors;
        laserEnabled = false;
        laserButton.GetComponent<Image>().sprite = laserButtonImageUnselected;



        PhotonNetwork.RaiseEvent(CLEAR_LINES_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

    }

    public void OnClick_DrawingButton()
    {
        //Debug.Log("In Drawing Button on click");
        if (!drawingEnabled)
        {
            //Debug.Log("enabling Pencil");
            drawButton.GetComponent<DrawingUIManager>().EnablePencil();
            drawingEnabled = true;
            drawButton.GetComponent<Image>().sprite = drawingButtonImageSelected;

        }
        else
        {
            DisableDrawing();
            PhotonNetwork.RaiseEvent(CLEAR_LINES_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        }
        DisableLaser();
        DisableHighlight();
    }

    public void DisableDrawing()
    {
        drawButton.GetComponent<DrawingUIManager>().DisablePencil();
        drawButton.GetComponent<Image>().sprite = drawingButtonImageUnselected;

        drawingEnabled = false;
    }

 

    public void SetGlobalCurrentCamera(string currentCamera)
    {
        globalCurrentCamera = currentCamera;
    }

    public string GetGlobalCurrentCamera()
    {
        return globalCurrentCamera;
    }
}
