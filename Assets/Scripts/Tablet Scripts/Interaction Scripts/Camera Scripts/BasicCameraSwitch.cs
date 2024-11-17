using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitch : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            camera1.SetActive(true);
            camera2.SetActive(false);    
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
        }
    }
}
