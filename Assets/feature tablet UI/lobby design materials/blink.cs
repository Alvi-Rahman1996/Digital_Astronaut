using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blink : MonoBehaviour
{
    public GameObject targetObject;
    public float repeatTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeStateOfGameObject", 1f, repeatTime);
    }

    void ChangeStateOfGameObject()
    {
        targetObject.SetActive(!targetObject.activeInHierarchy);
    }
}
