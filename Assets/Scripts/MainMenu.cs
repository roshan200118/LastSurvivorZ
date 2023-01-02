using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class to handle main manu
public class MainMenu : MonoBehaviour
{
    private AudioController audioController;

    private void Start()
    {
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
        audioController.HomeBackgroundAudio(true);
    }

    public void StartGame()
    {
        //Loads the first level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        //Close the application
        Application.Quit();
    }
}
