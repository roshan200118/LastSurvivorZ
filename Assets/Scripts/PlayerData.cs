using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to store Player Data
public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int maxHealth = 100;
    public int xp = 0;
    public int coins = 0;
    public bool regen = false;
    public bool poison = false;
    public bool freezing = false;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetData()
    {
        maxHealth = 100;
        xp = 0;
        coins = 0;
        regen = false;
        poison = false;
        freezing = false;
        PlayerController.smallAmmo = 32;
        PlayerController.medAmmo = 60;
        PlayerController.largeAmmo = 100;
    }
}
