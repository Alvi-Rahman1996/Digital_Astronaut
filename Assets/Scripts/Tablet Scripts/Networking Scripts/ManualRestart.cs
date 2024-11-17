using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualRestart : MonoBehaviour
{
    GameObject eventListener;

    private void Awake()
    {
        eventListener = GameObject.FindGameObjectWithTag("VRPlayer").gameObject;
    }
    public void OnClickRestart()
    {
        eventListener.GetComponent<EventListener>().ManualRestart();
    }
}
