using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Types;

//Class to keep track of ammo
public class AmmoCounter : MonoBehaviour
{
    //Reference for UI Text to display ammo count
    public TextMeshProUGUI smallCount;
    public TextMeshProUGUI medCount;
    public TextMeshProUGUI largeCount;
    public TextMeshProUGUI magCount;

    //Method to set ammo
    public void setAmmo(AmmoType ammoType, int count)
    {
        if (ammoType == AmmoType.SMALL)
        {

            smallCount.text = count.ToString();
        }
        else if (ammoType == AmmoType.MEDIUM)
        {
            medCount.text = count.ToString();
        }
        else if (ammoType == AmmoType.LARGE)
        {
            largeCount.text = count.ToString();
        }
    }

    //Method to set the Mag size
    public void setMag(int count)
    {
        magCount.text = count.ToString();
    }
}
