using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] bool syncDemoOn;
    // this class needs to store which OS is currently in use, and communicate that to the player manager, which will then create different player controllers for each player.
    public static RoomManager Instance;
    void Awake()
    {
        //Debug.Log("sync demo is " + syncDemoOn);

        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if((scene.name == "OuterSpaceSceneTabletTests" || scene.name == "OuterSpaceScene" || scene.name == "voice test") && syncDemoOn)
        {
            //Debug.Log("Inside Onsceneloaded and sync demo is " + syncDemoOn);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
