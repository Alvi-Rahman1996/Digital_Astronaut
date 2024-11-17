using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tab_options : MonoBehaviour
{
    public GameObject option_area;
    public void whenButtonClick()
    {
        
        if (option_area.activeInHierarchy == true)
            option_area.SetActive(false);
        else
            option_area.SetActive(true);
    }
}
