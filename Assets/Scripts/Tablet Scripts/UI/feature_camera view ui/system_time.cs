using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class system_time : MonoBehaviour
{
    public GameObject theDisplay;
    int hour;
    int minuts;
    int seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hour = System.DateTime.Now.Hour;
        minuts = System.DateTime.Now.Minute;
        seconds = System.DateTime.Now.Second;
        theDisplay.GetComponent<TMP_Text>().text = "" + hour + ":" + minuts + ":" + seconds;
    }
}
