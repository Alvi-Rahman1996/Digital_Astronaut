using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserUIManager : MonoBehaviour
{
    public GameObject mainCameraLaser;
    public GameObject helmetCameraLaser;
    public GameObject shoulderCameraLaser;

    void Start()
    {
        mainCameraLaser.SetActive(false);
        helmetCameraLaser.SetActive(false);
        shoulderCameraLaser.SetActive(false);
    }
    
    public void DisableLaser()
    {
        mainCameraLaser.SetActive(false);
        helmetCameraLaser.SetActive(false);
        shoulderCameraLaser.SetActive(false);
    }

    public void EnableLaser(string currentCamera)
    {
        if (currentCamera == "Main")
        {
            mainCameraLaser.SetActive(true);
            helmetCameraLaser.SetActive(false);
            shoulderCameraLaser.SetActive(false);
        }
        else if (currentCamera == "Helmet")
        {
            mainCameraLaser.SetActive(false);
            helmetCameraLaser.SetActive(true);
            shoulderCameraLaser.SetActive(false);
        }
        else if(currentCamera == "Shoulder")
        {
            mainCameraLaser.SetActive(false);
            helmetCameraLaser.SetActive(false);
            shoulderCameraLaser.SetActive(true);
        }
        else if (currentCamera == "None")
        {
            DisableLaser();
        }
    }

}
