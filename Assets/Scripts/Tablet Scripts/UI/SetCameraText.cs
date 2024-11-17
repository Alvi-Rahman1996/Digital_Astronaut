using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SetCameraText : MonoBehaviour
{
  
    public GameObject text;
    public GameObject whiteNoise;
    public float whiteNoiseDuration = 0.5f;
    public string Helmatcameratext = "name";
    public string Shouldercameratext = "name";
    public string Maincameratext = "name";
    public void onclick_helmetCameraButton()
    {
        if (text.GetComponent<TMP_Text>())
            //Debug.Log("I have TMP_Text");


        text.GetComponent<TMP_Text>().text = Helmatcameratext;
        ShowWhiteNoise();
    }
    public void onclick_ShoulderCameraButton()
    {
        
        text.GetComponent<TMP_Text>().text = Shouldercameratext;
        //text.GetComponent<Text>().text = Shouldercameratext;
        ShowWhiteNoise();
    }
    public void onclick_MainCameraButton()
    {
        text.GetComponent<TMP_Text>().text = Maincameratext;
        //text.GetComponent<Text>().text = Maincameratext;
        ShowWhiteNoise();

        
    }

    void ShowWhiteNoise()
    {
        StartCoroutine(WhiteNoiseCoroutine(whiteNoiseDuration));
    }

    IEnumerator WhiteNoiseCoroutine(float delay)
    {
        whiteNoise.SetActive(true);
        yield return new WaitForSeconds(delay);
        whiteNoise.SetActive(false);
    }
}
