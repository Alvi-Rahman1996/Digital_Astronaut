using Photon.Voice.Unity.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraInteraction : MonoBehaviour
{
    public Canvas canvas;

    [SerializeField] Sprite cameraButtonImageUnselected, cameraButtonImageSelected;

    public RawImage externalISSCamera1Image;
    public RawImage helmetCameraImage;
    public RawImage externalISSCamera2Image;

    public GameObject cameraContainer;
    public GameObject sceneCamera;
    public GameObject astroTalkAnim;
    public GameObject grayOutAstroTalkAnimeButton;
    GameObject externalISSCamera1 = null;
    GameObject helmetCamera = null;
    GameObject externalISSCamera2 = null;

    GameObject VRPlayer;
    GameObject TabletPlayer;

    private string currentFocusedCamera = "None";
    private string prevFocusedCamera = "None";
    private bool isCameraViewUp = true;
    private bool shouldAnimeRender = true;

    public List<Button> cameraButtons;
    public Button BackButton;

    private void Start()
    {
        SetExternal1Camera(GameObject.FindGameObjectWithTag("VR ISS Camera 1"));
        SetExternal2Camera(GameObject.FindGameObjectWithTag("VR ISS Camera 2"));
        Debug.Log(GameObject.FindGameObjectWithTag("VR ISS Camera 2").name);
    }


    public void SetHelmetCamera(GameObject helmetCamera)
    {
        this.helmetCamera = helmetCamera;
    }
    public void SetExternal2Camera(GameObject shoulderCamera)
    {
        this.externalISSCamera2 = shoulderCamera;
    }
    public void SetExternal1Camera(GameObject mainCamera)
    {
        this.externalISSCamera1 = mainCamera;
    }
    public void SetVRPlayer(GameObject VRPlayer)
    {
        this.VRPlayer = VRPlayer;
    }
    public void SetTabletPlayer(GameObject TabletPlayer)
    {
        this.TabletPlayer = TabletPlayer;
    }

    public string GetCurrentCamera()
    {
        return currentFocusedCamera;
    }

    public void OnClick_ToggleAstroAnim()
    {
        grayOutAstroTalkAnimeButton.SetActive(shouldAnimeRender);
        shouldAnimeRender = !shouldAnimeRender;
        if(currentFocusedCamera == "Helmet" || prevFocusedCamera == "Helmet")
            astroTalkAnim.SetActive(shouldAnimeRender);

    }
    public void OnClick_MainCamera()
    {
        //set size
        externalISSCamera1Image.GetComponent<RectTransform>().sizeDelta = new Vector2(2100, 1150);

        //set Anchor to Center
        externalISSCamera1Image.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        externalISSCamera1Image.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

        //set postion
        externalISSCamera1Image.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        foreach (Button camButton in cameraButtons)
        {
            camButton.gameObject.SetActive(false);
        }

        helmetCameraImage.gameObject.SetActive(false);
        externalISSCamera2Image.gameObject.SetActive(false);

        sceneCamera.GetComponent<HelmetSync>().UntrackHMD();

        externalISSCamera1.transform.GetPositionAndRotation(out Vector3 outPost, out Quaternion outRotation);
        sceneCamera.transform.SetPositionAndRotation(outPost, outRotation);

        TabletPlayer.transform.SetParent(externalISSCamera1.transform);



        BackButton.interactable = true;
        currentFocusedCamera = "Main";
        prevFocusedCamera = "None";
        BackButton.GetComponent<Image>().sprite = cameraButtonImageUnselected;

        cameraContainer.SetActive(false);
        astroTalkAnim.SetActive(false);
        isCameraViewUp = false;


        canvas.GetComponent<TabletUIManager>().SetGlobalCurrentCamera(currentFocusedCamera);
        canvas.GetComponent<TabletUIManager>().EnableAll();
    }

    public void OnClick_HelmetCamera()
    {
        SetTabletPlayer(GameObject.FindGameObjectWithTag("TabletPlayer"));
        SetVRPlayer(GameObject.FindGameObjectWithTag("VRPlayer"));

        //TabletPlayer.transform.SetParent(VRPlayer.transform);

        SetHelmetCamera(GameObject.FindGameObjectWithTag("VR Helmet Camera"));

        //set size
        helmetCameraImage.GetComponent<RectTransform>().sizeDelta = new Vector2(2100, 1150);

        //set Anchor to Center
        helmetCameraImage.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        helmetCameraImage.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

        //set postion
        helmetCameraImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);


        foreach (Button camButton in cameraButtons)
        {
            camButton.gameObject.SetActive(false);
        }

        externalISSCamera1Image.gameObject.SetActive(false);
        externalISSCamera2Image.gameObject.SetActive(false);

        helmetCamera.transform.GetPositionAndRotation(out Vector3 outPost, out Quaternion outRotation);
        sceneCamera.transform.SetPositionAndRotation(outPost, outRotation);

        

        BackButton.interactable = true;
        currentFocusedCamera = "Helmet";
        prevFocusedCamera = "None";
        BackButton.GetComponent<Image>().sprite = cameraButtonImageUnselected;


        sceneCamera.GetComponent<HelmetSync>().TrackHMD();

        cameraContainer.SetActive(false);
        if (shouldAnimeRender)
            astroTalkAnim.SetActive(true);
        isCameraViewUp = false;

        canvas.GetComponent<TabletUIManager>().SetGlobalCurrentCamera(currentFocusedCamera);
        canvas.GetComponent<TabletUIManager>().EnableAll();


    }

    public void OnClick_ShoulderCamera()
    {
        SetTabletPlayer(GameObject.FindGameObjectWithTag("TabletPlayer"));
        SetVRPlayer(GameObject.FindGameObjectWithTag("VRPlayer"));

        //TabletPlayer.transform.SetParent(VRPlayer.transform);


        //set size
        externalISSCamera2Image.GetComponent<RectTransform>().sizeDelta = new Vector2(2100, 1150);

        //set Anchor to Center
        externalISSCamera2Image.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        externalISSCamera2Image.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

        //set postion
        externalISSCamera2Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

        foreach (Button camButton in cameraButtons)
        {
            camButton.gameObject.SetActive(false);
        }
        externalISSCamera1Image.gameObject.SetActive(false);
        helmetCameraImage.gameObject.SetActive(false);

        externalISSCamera2.transform.GetPositionAndRotation(out Vector3 outPost, out Quaternion outRotation);
        sceneCamera.transform.SetPositionAndRotation(outPost, outRotation);
        TabletPlayer.transform.SetParent(externalISSCamera2.transform);

        //Makes it move with the camera
        //TabletPlayer.transform.SetParent(shoulderCamera.transform);
        sceneCamera.GetComponent<HelmetSync>().UntrackHMD();


        BackButton.interactable = true;
        currentFocusedCamera = "Shoulder";
        prevFocusedCamera = "None";
        BackButton.GetComponent<Image>().sprite = cameraButtonImageUnselected;


        cameraContainer.SetActive(false);
        astroTalkAnim.SetActive(false);
        isCameraViewUp = false;


        canvas.GetComponent<TabletUIManager>().SetGlobalCurrentCamera(currentFocusedCamera);
        canvas.GetComponent<TabletUIManager>().EnableAll();

    }

    public void OnClick_Back()
    {
        SetTabletPlayer(GameObject.FindGameObjectWithTag("TabletPlayer"));
        SetVRPlayer(GameObject.FindGameObjectWithTag("VRPlayer"));

        //TabletPlayer.transform.SetParent(null);


        foreach (Button camButton in cameraButtons)
        {
            if (currentFocusedCamera == "Main")
            {
                prevFocusedCamera = "Main";

                externalISSCamera1Image.GetComponent<RectTransform>().sizeDelta = new Vector2(2100, 575);

                externalISSCamera1Image.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
                externalISSCamera1Image.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);

                externalISSCamera1Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -790, 0);
            }
            else if (currentFocusedCamera == "Helmet")
            {
                prevFocusedCamera = "Helmet";

                helmetCameraImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1050, 575);

                helmetCameraImage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                helmetCameraImage.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);

                helmetCameraImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(-658, -360, 0);
            }
            else if (currentFocusedCamera == "Shoulder")
            {
                prevFocusedCamera = "Shoulder";

                externalISSCamera2Image.GetComponent<RectTransform>().sizeDelta = new Vector2(1050, 575);

                externalISSCamera2Image.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                externalISSCamera2Image.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);

                externalISSCamera2Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(656, -360, 0);
            }
            camButton.gameObject.SetActive(true);
            currentFocusedCamera = "None";


            if (isCameraViewUp)
            {
                cameraContainer.SetActive(false);
                BackButton.GetComponent<Image>().sprite = cameraButtonImageUnselected;
                canvas.GetComponent<TabletUIManager>().EnableAll();

            }
            else
            {
                cameraContainer.SetActive(true);
                BackButton.GetComponent<Image>().sprite = cameraButtonImageSelected;
                canvas.GetComponent<TabletUIManager>().DisableAll();

            }


            canvas.GetComponent<TabletUIManager>().SetGlobalCurrentCamera(currentFocusedCamera);
            isCameraViewUp = !isCameraViewUp;
        }

        externalISSCamera1Image.gameObject.SetActive(true);
        helmetCameraImage.gameObject.SetActive(true);
        externalISSCamera2Image.gameObject.SetActive(true);
    }
}
