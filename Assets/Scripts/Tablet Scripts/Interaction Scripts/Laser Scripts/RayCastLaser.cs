using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class RayCastLaser : MonoBehaviour
{
    public Camera playerCamera;
    public Transform laserOrigin;
    public float laserRange = 200f;
    public float laserDelay= 0.1f;
    private Vector3 mousePos;
    private List<Vector3> startEndPoints = new List<Vector3>();

    const byte LASER_EVENT = 1;

    LineRenderer laserLine;
    private int clicks = 0;


    // Start is called before the first frame update
    void Awake()
    {
     laserLine = GetComponent<LineRenderer>();   
    }


    void Start()
    {
    }

    private void OnEnable()
    {
        if (laserLine != null) {
            laserLine.SetPosition(0, new Vector3(0,0,0));
            laserLine.SetPosition(1, new Vector3(0,0,0));
        }
    }

    IEnumerator ToggleLaser(float delay, Vector3 pos)
    {    
        yield return new WaitForSeconds(delay);

        laserLine.SetPosition(1, pos);
        startEndPoints.Add(pos);
        object[] data = startEndPoints.Cast<object>().ToArray();
        PhotonNetwork.RaiseEvent(LASER_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startEndPoints.Clear();
            mousePos = Input.mousePosition;
            mousePos.z = 5f;
            laserLine.SetPosition(0, laserOrigin.position);
            startEndPoints.Add(laserOrigin.position);
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(mousePos);
            Vector3 finalPos = new Vector3();
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit))
            {
                finalPos = hit.point;

            }
            else
            {
                finalPos = ray.GetPoint(laserRange);
            }


            StartCoroutine(ToggleLaser(laserDelay, finalPos));

            clicks++;
        }

    }
}
