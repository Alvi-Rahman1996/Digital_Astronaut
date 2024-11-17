using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            CloseMenu(menus[i]);
        }
        OpenMenu("temp");
    }

    public void OpenMenu(string menuName)
    {
        //Debug.Log("Called open menu with string " + menuName);
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
                //Debug.Log("Closing menu " + menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                //Debug.Log("Closing menu " + menus[i]);
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
