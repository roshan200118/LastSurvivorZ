using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

//Class to handle ammo pickup
public class AmmoPickup : MonoBehaviour
{
    public AmmoType ammoType;
    private AudioController audioController;

    private void Start()
    {
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Add ammo if player picks up ammo
        if (other.transform.GetComponentInParent<PlayerController>())
        {
            audioController.PickupAmmoAudio();
            PlayerController.instance.addAmmo(ammoType, 8);
            Destroy(gameObject);
        }
    }
}
