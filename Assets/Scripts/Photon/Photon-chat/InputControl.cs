using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public GameObject ChatManagerPrefab; // Reference to the ChatUI prefab
    private GameObject ChatManagerInstance; // Reference to the instantiated ChatUI

    // Start is called before the first frame update
    void Start()
    {
       ChatManagerInstance = Instantiate(ChatManagerPrefab);
       ChatManagerInstance.SetActive(false); // Start with the ChatUI disabled
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
    {
        ChatManagerInstance.SetActive(!ChatManagerInstance.activeSelf);
    }

    }
}
