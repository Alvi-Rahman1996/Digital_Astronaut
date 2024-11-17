using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExitGames;
using ExitGames.Client.Photon;

public class DrawLine : MonoBehaviour
{
    public Camera cam = null;
    public LineRenderer lineRenderer = null;
    private Vector3 mousePos;
    private Vector3 pos;
    private Vector3 previousPos;
    public List<Vector3> LinePosition = new List<Vector3>();
    public float minimumDistance = 0.05f;
    public float depth = 10;
    private float distance = 0;

    const byte DRAW_EVENT = 0;
    PhotonView PV;

    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();    }

    public void Clear()
    {
        LinePosition.Clear();
        lineRenderer.SetPositions(LinePosition.ToArray());
    }

    private void OnDisable()
    {
        LinePosition.Clear();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LinePosition.Clear();
            mousePos = Input.mousePosition;
            mousePos.z = depth;
            pos = cam.ScreenToWorldPoint(mousePos);
            previousPos = pos;
            LinePosition.Add(pos);


            object[] data = new object[] {};

            foreach (Vector3 vect in LinePosition)
            {
                data.Append(vect.x);
                data.Append(vect.y);
                data.Append(vect.z);
            }
            PhotonNetwork.RaiseEvent(DRAW_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
        else if (Input.GetMouseButton(0))
        {
            mousePos = Input.mousePosition;
            mousePos.z = depth;
            pos = cam.ScreenToWorldPoint(mousePos);
            distance = Vector3.Distance(pos, previousPos);
            if (distance >= minimumDistance)
            {
                previousPos = pos;
                LinePosition.Add(pos);
                lineRenderer.positionCount = LinePosition.Count;
                lineRenderer.SetPositions(LinePosition.ToArray());
            }
            Debug.Log("Running in GetMouseButton(0)");
            object[] data = LinePosition.Cast<object>().ToArray();

            PhotonNetwork.RaiseEvent(DRAW_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }
}
