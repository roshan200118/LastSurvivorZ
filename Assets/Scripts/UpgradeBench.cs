using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Method to handle Upgrade Bench behaviour
public class UpgradeBench : MonoBehaviour
{
    public LayerMask playerMask;
    public GameObject instructions;

    private void Update()
    {
        CheckNearBench();
    }

    //Method to check if player is near the bench
    private void CheckNearBench()
    {
        bool playerInRange = Physics.CheckSphere(transform.position, 3, playerMask);

        if (playerInRange)
        {
            instructions.SetActive(true);
        }
        else
        {
            instructions.SetActive(false);
        }

        //If player decides to upgrade weapon
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && PlayerController.instance.equippedWeapon != null && PlayerController.instance.coins >= 5)
        {
            PlayerController.instance.SpendCoins(5);
            PlayerController.instance.equippedWeapon.magSize += 6;
        }
    }
}
