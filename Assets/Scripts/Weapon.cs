using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using TMPro;

//Method to handle the Weapon
public class Weapon : MonoBehaviour
{
    Weapon thisWeapon;
    Rigidbody rb;
    BoxCollider coll;
    GameObject playerObj;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public AmmoType ammoType;
    public AmmoCounter ammoCounter;
    public GameObject icon;
    public LayerMask playerMask, enemyMask;
    public GameObject pickupInstructionsText;
    public GameObject explosionFX;
    public GameObject bloodFX;
    private Transform weaponHolder;
    private AudioController audioController;

    public int damage = 20;
    public int magSize = 16;
    public int curAmmo;
    public float range = 30f;
    public float reloadTime = 1f;
    public float pickUpRange = 2f;
    public float dropForce = 1f;

    bool playerInRange = false;
    bool isEquipped = false;
    bool isReloading = false;

    private void Awake()
    {
        thisWeapon = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        weaponHolder = playerObj.transform.Find("WeaponHolder");

        if (PlayerController.instance.classType == ClassType.TINKERER)
        {
            damage = Mathf.FloorToInt(damage * 1.5f);
        }

        rb = GetComponent<Rigidbody>();
        coll = GetComponentInChildren<BoxCollider>();

        curAmmo = magSize;
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEquipped) CheckPickUp();

        if (isEquipped && !pickupInstructionsText.activeInHierarchy && curAmmo <= 0)
        {
            pickupInstructionsText.SetActive(true);
            pickupInstructionsText.GetComponent<TextMeshProUGUI>().text = "Press R to reload";
        }

        if (Input.GetKeyDown(KeyCode.R) && isEquipped)
        {
            StartCoroutine(Reload());
        }
    }

    private void CheckPickUp()
    {
        playerInRange = Physics.CheckSphere(transform.position, pickUpRange, playerMask);

        if (playerInRange && !isEquipped)
        {
            pickupInstructionsText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                icon.SetActive(true);
                isEquipped = true;
                PlayerController.instance.PickUpWeapon(thisWeapon);

                transform.SetParent(weaponHolder);

                transform.Find("StarEffect").gameObject.SetActive(false);

                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
                transform.localScale = Vector3.one;

                if (transform.name.Equals("RPG"))
                {
                    transform.Rotate(0f, 180f, 0, Space.Self);
                }

                rb.isKinematic = true;
                coll.isTrigger = true;

                ammoCounter.setMag(curAmmo);
                pickupInstructionsText.SetActive(false);
            }
        }
        else
        {
            pickupInstructionsText.SetActive(false);
        }
    }

    public void Drop()
    {
        icon.SetActive(false);
        isEquipped = false;
        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        ammoCounter.setMag(0);

        rb.velocity = playerObj.GetComponent<Rigidbody>().velocity;

        //Add a force when dropping the weapon
        rb.AddForce(playerObj.transform.forward * dropForce, ForceMode.Impulse);
        rb.AddForce(playerObj.transform.up * dropForce, ForceMode.Impulse);

        //Give the weapon a random torque (for dropping effect)
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        transform.Find("StarEffect").gameObject.SetActive(true);
    }

    public void Shoot(bool poison, bool freezing)
    {
        if (curAmmo > 0 && !isReloading)
        {
            curAmmo--;
            ammoCounter.setMag(curAmmo);

            muzzleFlash.Play();
            audioController.GunAudio(ammoType);
            RaycastHit hit;
            RaycastHit hit2;

            if (ammoType == AmmoType.LARGE && Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit2, range))
            {
                GameObject explosionGO_FX = Instantiate(explosionFX, hit2.point, Quaternion.identity);
                Destroy(explosionGO_FX, 1.5f);
            }

            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, range, enemyMask))
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                GameObject bloodGO_FX = Instantiate(bloodFX, hit.point, Quaternion.Euler(new Vector3(0, 0, 90)));
                if (poison)
                {
                    enemy.Poison();
                }

                if (freezing)
                {
                    enemy.Freeze();
                }
                enemy.takeDamage(damage);
                Destroy(bloodGO_FX, 0.2f);
            }
        }
    }

    public IEnumerator Reload()
    {

        if (!isReloading)
        {
            isReloading = true;

            int ammoRemaining = PlayerController.instance.checkAmmo(ammoType);
            int ammoTaken = magSize - curAmmo;

            if (ammoRemaining >= ammoTaken)
            {
                audioController.PickupWeaponAudio();
                yield return new WaitForSeconds(reloadTime);

                curAmmo = magSize;
                ammoCounter.setMag(curAmmo);
                PlayerController.instance.addAmmo(ammoType, -ammoTaken);
            }
            else if (ammoRemaining != 0)
            {
                audioController.PickupWeaponAudio();
                yield return new WaitForSeconds(reloadTime);

                curAmmo = curAmmo + ammoRemaining;
                ammoCounter.setMag(curAmmo);
                PlayerController.instance.addAmmo(ammoType, -ammoRemaining);
            }
            pickupInstructionsText.SetActive(false);
            pickupInstructionsText.GetComponent<TextMeshProUGUI>().text = "Press E to pick up the gun";
            isReloading = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

