using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class to handle Pause Menu behavior
public class PauseMenu : MonoBehaviour
{
    public GameObject skillTreeObj;
    public SkillTree skillTree;

    //Method to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1;
        PlayerController.hasCarKey = false;
        PlayerController.hasCityKey1 = false;
        PlayerController.hasCityKey2 = false;
        PlayerController.hasCityKey3 = false;
        PlayerController.instance.hasPotion = false;
        PlayerController.level0Complete = false;
        SceneManager.LoadScene(0);
        PlayerData.instance.ResetData();
    }

    //Method to open the skill tree
    public void OpenSkillTree()
    {
        skillTreeObj.SetActive(true);
        skillTree.UpdateButtons();
    }

    //Method to quiet the game
    public void QuitGame()
    {
        //Close the application
        Application.Quit();
    }
}
