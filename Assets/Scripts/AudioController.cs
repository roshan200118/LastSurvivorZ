using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types; //For ammo sound

//Class to manage audio
public class AudioController : MonoBehaviour
{
    public AudioSource carAudio;
    public AudioSource homeBackgroundAudio;
    public AudioSource playerMovement; //Level 0 is running on grass and Level 1 is running on concrete
    public AudioSource playerDamage;
    public AudioSource zombieAttack;
    public AudioSource zombieDamage;
    public AudioSource carHitZombie;
    public AudioSource weaponPickup;
    public AudioSource coinPickup;
    public AudioSource itemPickup;
    public AudioSource ammoPickup;
    public AudioSource[] gunShots;// = [ smallest, ... , largest];

    //Plays when the player is in the car and loops once finished playing
    public void CarAudio(bool inCar)
    {
        if (inCar)
        {
            if (!carAudio.isPlaying)
            {
                carAudio.Play();
            }
        }
        else
        {
            carAudio.Stop();
        }
    }

    //Plays home background music
    public void HomeBackgroundAudio(bool isPlaying)
    {
        if (isPlaying)
        {
            if (!homeBackgroundAudio.isPlaying)
            {
                homeBackgroundAudio.Play();
            }
        }
        else
        {
            homeBackgroundAudio.Stop();
        }
    }

    //Changes based on gun size ext
    public void GunAudio(AmmoType ammoType)
    {
        //Stops all guns from shooting noise
        foreach (AudioSource a in gunShots)
        {
            if (a.isPlaying)
            {
                a.Stop();
            }
        }

        switch (ammoType)
        {
            case AmmoType.LARGE:
                gunShots[2].Play();
                break;
            case AmmoType.MEDIUM:
                gunShots[1].Play();
                break;
            default:
                gunShots[0].Play();
                break;
        }

    }

    //If the player is moving the walking sound is played
    public void PlayerMovementAudio(bool isMoving)
    {
        if (isMoving)
        {
            if (!playerMovement.isPlaying)
            {
                playerMovement.Play();
            }
        }
        else
        {
            playerMovement.Stop();
        }
    }

    //Plays the player hurt audio as well as the zombie attack sound
    public void PlayerDamageAudio()
    {
        if (playerDamage.isPlaying)
        {
            playerDamage.Stop();
        }

        playerDamage.Play();
    }

    //Plays if car hits zombie
    public void CarHitZombieAudio()
    {
        if (carHitZombie.isPlaying)
        {
            carHitZombie.Stop();
        }

        carHitZombie.Play();
    }

    //Plays the player hurt audio as well as the zombie attack sound
    public void ZombieAttackAudio()
    {
        if (zombieAttack.isPlaying)
        {
            zombieAttack.Stop();
        }

        zombieAttack.Play();
    }
    //Plays the zombie hurt audio, stops overlap
    public void ZombieDamageAudio()
    {
        if (zombieDamage.isPlaying)
        {
            zombieDamage.Stop();
        }

        zombieDamage.Play();
    }

    //Plays audio for weapon pickup
    public void PickupWeaponAudio()
    {
        if (!weaponPickup.isPlaying)
        {
            weaponPickup.Play();
        }
    }

    //Plays audio for coin pickup
    public void CoinAudio()
    {
        if (!coinPickup.isPlaying)
        {
            coinPickup.Play();
        }
    }

    //Plays audio for item pickup
    public void PickupItemAudio()
    {
        if (!itemPickup.isPlaying)
        {
            itemPickup.Play();
        }
    }

    //Plays audio for ammo pickup
    public void PickupAmmoAudio()
    {
        if (!ammoPickup.isPlaying)
        {
            ammoPickup.Play();
        }
    }
}
