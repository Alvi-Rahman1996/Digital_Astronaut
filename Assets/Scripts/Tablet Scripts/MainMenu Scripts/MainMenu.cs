using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;


public class MainMenu : MonoBehaviourPunCallbacks
{
    public string firstLevel;
    public GameObject optionsScreen;

    //Login Variables
    public GameObject loginScreen;
    public GameObject passwordTextfield;

    //Room Variables
    public GameObject connectedText;
    public GameObject connectingText;
    public GameObject loadingCircle;
    public GameObject startButton;


    private int roomNumber = 1;
    private int setPassLength = 0;
    private string pass = "";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        setPassLength = Random.Range(8, 12);
        PhotonNetwork.GameVersion = "0.0.1";
        //Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
        //InvokeRepeating("ChangeStateOfGameObject", 1f, 0.7f);

    }

    //Photon Overides
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("In launcher opening temp");
        
        Debug.Log("Joined lobby");
    }

    public override void OnLeftRoom()
    {
        pass = "";
        passwordTextfield.GetComponent<TMP_InputField>().text = "";

        loginScreen.transform.DOMoveX(1440, 0.5f);
    }

    public override void OnJoinedRoom()
    {
        loginScreen.transform.DOMoveX(6000, 0.5f);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        connectedText.SetActive(true);
        startButton.SetActive(true);

        CancelInvoke();
        connectingText.SetActive(false);
        loadingCircle.SetActive(false);
    }


    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        connectedText.SetActive(false);
        startButton.SetActive(false);

        //InvokeRepeating("ChangeStateOfGameObject", 1f, 0.7f);
        connectingText.SetActive(true);
        loadingCircle.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomNumber++;
        PhotonNetwork.CreateRoom("Room " + roomNumber);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //Onclicks
    public void OnClick_Login()
    {
        if(passwordTextfield.GetComponent<TMP_InputField>().text == "")
        {
            StartCoroutine(FakePassword(Random.Range(0.08f, 0.12f)));
        }
        else
        {
            PhotonNetwork.CreateRoom("Room " + roomNumber);
        }
    }

    public void OnClick_LeaveRoom()
    {
        //Debug.Log(PhotonNetwork.CurrentRoom);
        PhotonNetwork.LeaveRoom(true);
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(1);
        }

    }




    //Local code
    IEnumerator FakePassword(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        pass += "*";
        passwordTextfield.GetComponent<TMP_InputField>().text = pass;

        if (pass.Length >= setPassLength)
        {
            PhotonNetwork.CreateRoom("Room " + roomNumber);

        }
        else
        {
            StartCoroutine(FakePassword(Random.Range(0.04f, 0.15f)));
        }
    }
    void ChangeStateOfGameObject()
    {
        connectingText.SetActive(!connectingText.activeInHierarchy);
    }
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Quitting");
    }
}

