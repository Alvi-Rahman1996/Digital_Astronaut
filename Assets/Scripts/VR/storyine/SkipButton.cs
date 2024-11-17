using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;


public class SkipButton : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string nextSceneName; // Name of the next scene to load
    public float autoProceedTime = 18.0f; // Time in seconds to wait before auto-proceeding

    private void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Stop the video
            playableDirector.Stop();

            // Load the next scene
            // SceneManager.LoadScene(nextSceneName);

            // This needs to be done through photon because otherwise they are in two separate scenes 
            // and only the master should load the next level otherwise the client will load into the
            // scene alone.
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(nextSceneName); 
            }
        }
    }

    private void Start()
    {
        // Start a coroutine to auto-proceed after a specified time
        StartCoroutine(AutoProceed());
    }

    private IEnumerator AutoProceed()
    {
        yield return new WaitForSeconds(autoProceedTime);

        // Load the next scene automatically after the specified time
        // SceneManager.LoadScene(nextSceneName);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(nextSceneName);
        }
    }
}
