using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySeparationManager : MonoBehaviour
{
    void Start()
    {
        if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene("Rooms");
        }
    }
}
