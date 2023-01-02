using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Types;

//Class to handle Class selection
public class ClassSelector : MonoBehaviour
{
    public static ClassSelector instance;
    public ClassType selectedClass;

    private AudioController audioController;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
        audioController.HomeBackgroundAudio(true);
    }

    //Method to assign the class the player chooses
    public void SelectClass(int classType)
    {
        switch (classType)
        {
            case 1:
                selectedClass = ClassType.TINKERER;
                break;
            case 2:
                selectedClass = ClassType.TANK;
                break;
            case 3:
                selectedClass = ClassType.ATHLETE;
                break;
        }

        audioController.HomeBackgroundAudio(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
