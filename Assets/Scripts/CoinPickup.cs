using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to handle coin pickup
public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<PlayerController>())
        {
            PlayerController.instance.PickUpCoin();
            Destroy(gameObject);
        }
    }
}
