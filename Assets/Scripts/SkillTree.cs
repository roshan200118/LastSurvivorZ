using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Class to handle Skill Tree behavior
public class SkillTree : MonoBehaviour
{
    public Button regenBtn, poisonBtn, freezingBtn;

    public GameObject skillTree;

    public int regenCost = 5;
    public int poisonCost = 8;
    public int freezingCost = 5;

    private void Start()
    {
        UpdateButtons();
    }

    //Method to show which skills are currently available to player
    public void UpdateButtons()
    {
        //Check if player has enough XP for regen
        if (PlayerController.xp < regenCost)
        {
            regenBtn.interactable = false;
        }
        else
        {
            regenBtn.interactable = true;
        }

        //Check if player has enough XP for poison
        if (PlayerController.xp < poisonCost)
        {
            poisonBtn.interactable = false;
        }
        else
        {
            poisonBtn.interactable = true;
        }

        //Check if player has enough XP for freezing
        if (PlayerController.xp < freezingCost)
        {
            freezingBtn.interactable = false;
        }
        else
        {
            freezingBtn.interactable = true;
        }

        //Checking if player has pre requisite
        if (!PlayerData.instance.regen)
        {
            poisonBtn.interactable = false;
            freezingBtn.interactable = false;
        }

        //Check if player has activated any of the skills
        if (PlayerData.instance.regen)
        {
            regenBtn.interactable = false;
            regenBtn.GetComponent<Image>().color = Color.green;
        }
        if (PlayerData.instance.poison)
        {
            poisonBtn.interactable = false;
            poisonBtn.GetComponent<Image>().color = Color.green;
        }
        if (PlayerData.instance.freezing)
        {
            freezingBtn.interactable = false;
            freezingBtn.GetComponent<Image>().color = Color.green;
        }
    }

    //Method to activate Regen skill
    public void activateRegen()
    {
        PlayerData.instance.regen = true;
        PlayerController.instance.addXP(-regenCost);
        UpdateButtons();
    }

    //Method to activate poison skill
    public void activatePoison()
    {
        PlayerData.instance.poison = true;
        PlayerController.instance.addXP(-poisonCost);
        UpdateButtons();
    }

    //Method to activate freezing skill
    public void activateFreezing()
    {
        PlayerData.instance.freezing = true;
        PlayerController.instance.addXP(-freezingCost);
        UpdateButtons();
    }

    //Method to close skill tree menu
    public void Back()
    {
        skillTree.SetActive(false);
    }
}
