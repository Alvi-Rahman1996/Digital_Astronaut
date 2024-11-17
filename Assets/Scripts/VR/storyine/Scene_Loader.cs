using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load

    private bool hasLoadedScene = false;

    private void Update()
    {
        if (gameObject.activeSelf && !hasLoadedScene)
        {
            LoadNextScene();
            hasLoadedScene = true;
        }
    }

    private void LoadNextScene()
    {
        //SceneManager.LoadScene(nextSceneName);
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(nextSceneName);
    }
}
