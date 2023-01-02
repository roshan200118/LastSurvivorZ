using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class to handle game ending
public class GameEnding : MonoBehaviour
{
    //Used to fade completetion screen after game done
    public CanvasGroup exitBackgroundImageCanvasGroup;

    //Used to handle fade effects
    public float fadeDuration = 2.5f;
    public float displayImageDuration = 1f;

    private AudioController audioController;

    //Timer for fade effects
    float timer;

    private void Start()
    {
        //Assign the exit background
        exitBackgroundImageCanvasGroup = GameObject.FindObjectOfType<CanvasGroup>();

        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    void Update()
    {
        //If player has potion, show win screen
        if (PlayerController.instance.hasPotion)
        {
            audioController.HomeBackgroundAudio(true);
            ShowLevel2WinScreenFade();
        }

        //If player is done level 1, show scene change screen
        if (PlayerController.instance.level1Complete)
        {
            audioController.PlayerMovementAudio(false);
            ShowLevel1WinScreenFade();
        }
    }

    //Method when Player wins the game, fade out and show completion screen
    public void ShowLevel0WinScreenFade()
    {
        timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = timer / fadeDuration;

        if (timer > fadeDuration + displayImageDuration)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //Method when Player wins the game, fade out and show completion screen
    public void ShowLevel1WinScreenFade()
    {
        Time.timeScale = 0;

        timer += Time.unscaledDeltaTime;

        exitBackgroundImageCanvasGroup.alpha = timer / fadeDuration;

        if (timer > fadeDuration + displayImageDuration)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //Method when Player wins the game, fade out and show completion screen
    void ShowLevel2WinScreenFade()
    {
        timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = timer / fadeDuration;

        if (timer > fadeDuration + displayImageDuration)
        {
            //Let the cursor show again
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
