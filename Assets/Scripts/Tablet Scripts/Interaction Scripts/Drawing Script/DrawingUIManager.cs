using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingUIManager : MonoBehaviour
{
    public GameObject pencil;

    public void EnablePencil()
    {
        pencil.SetActive(true);
    }

    public void DisablePencil()
    {
        Debug.Log("Disabling Pencil");
        pencil.GetComponent<DrawLine>().Clear();
        pencil.SetActive(false);

    }
}
