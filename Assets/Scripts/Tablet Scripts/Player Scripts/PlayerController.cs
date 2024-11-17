using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject cameraHolder, helmetCamera;
    [SerializeField] float mouseSenesitivity, walkSpeed, smoothTime;
    [SerializeField] GameObject ui;



    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    private GameObject VRPlayerCamera;

    Rigidbody rb;

    PhotonView PV;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        Look();
        Move();
        
    }
    private void Start()
    {
        //Debug.Log("In PC, Is this view mine? " + PV.IsMine);
        //Debug.Log("In PC, is Master client? " + PhotonNetwork.IsMasterClient);


        GameObject mainCameraScene = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject astronautScene = GameObject.FindGameObjectWithTag("astronout");

        //This is for removing duplicated GO
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            ui.SetActive(false);
            cameraHolder.SetActive(false);
            //Removing tablet player model from client side
        }
        // This is for the VR Player
        else if(!PhotonNetwork.IsMasterClient)
        {
            mainCameraScene.GetComponent<PhotonView>().TransferOwnership(PV.Owner);
            astronautScene.GetComponent<PhotonView>().TransferOwnership(PV.Owner);
            ui.SetActive(false);
            cameraHolder.SetActive(false);
            helmetCamera.SetActive(false);
            this.gameObject.tag = "VRPlayer";
        }
        // This is the Tablet Player
        if(PV.IsMine && PhotonNetwork.IsMasterClient)
        {
            //helmetCamera.transform.SetParent(mainCameraScene.transform);
            //shoulderCamera.transform.SetParent(astronautScene.transform);
            //bodyCameraholder.SetActive(false);

            // remove the VR linerendering camera for the Tablet player
            //GameObject.FindGameObjectWithTag("LineRenderCamera").SetActive(false);
            VRPlayerCamera =  GameObject.FindGameObjectWithTag("MainCamera");
            VRPlayerCamera.SetActive(false);
            //Invoke("RenableCamera", 2.5f);

        }
    }

    void RenableCamera()
    {
        VRPlayerCamera.SetActive(true);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSenesitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSenesitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;   
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * walkSpeed, ref smoothMoveVelocity, smoothTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount)* Time.fixedDeltaTime);
    }
}
