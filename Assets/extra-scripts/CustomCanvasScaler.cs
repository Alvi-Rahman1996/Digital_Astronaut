using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCanvasScaler : MonoBehaviour
{
    public GameObject mainCanvas;

    private Vector2 mainCanvasSize;
    //private Vector3 mainCanvasScale;
    void FixedUpdate()
    {
        mainCanvasSize = mainCanvas.GetComponent<RectTransform>().sizeDelta;
        //mainCanvasScale = mainCanvas.GetComponent<RectTransform>().localScale;
        this.GetComponent<RectTransform>().sizeDelta = mainCanvasSize;
        //this.GetComponent<RectTransform>().localScale = mainCanvasScale;
    }
}
