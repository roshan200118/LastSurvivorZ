using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to handle weapon pickup
public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<PlayerController>())
        {
            PlayerController.instance.PickUpWpnPart();
            Destroy(gameObject);
        }
    }
}
