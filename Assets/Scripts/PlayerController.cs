using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Types;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private CameraController cam;
    private Rigidbody rb;
    private AudioController audioController;
    Animator animator;

    public Weapon equippedWeapon;
    public LayerMask groundMask;
    public GameObject pauseMenu;
    public GameObject finalWeapon;
    public TextMeshProUGUI xpCount, coinCount;
    public HealthBar healthBar;
    public AmmoCounter ammoCounter;
    public ClassType classType;

    public int maxHealth;
    public float moveSpeed = 5.0f;
    public float jumpForce = 1.0f;
    public float regenTime = 5.0f;
    float regenTimer = 0;
    float x, z;

    public int coins = 0;
    public static int xp = 0;
    public static int smallAmmo = 32;
    public static int medAmmo = 60;
    public static int largeAmmo = 100;
    static int health;
    int wpnPartsCollected = 0;
    bool grounded = true;
    bool menuUp = false;
    public bool regen, poison, freezing;    // Skill tree skills
    public bool hasPotion = false;
    public bool isPlayerDriving = false;
    public bool level1Complete = false;

    public static bool hasCarKey = false;
    public static bool hasCityKey1 = false;
    public static bool hasCityKey2 = false;
    public static bool hasCityKey3 = false;
    public static bool level0Complete = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the Singleton
        }
        else
        {
            Debug.LogError("Player.Awake() - Attempted to assign second Player.S!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set class type
        if (ClassSelector.instance) classType = ClassSelector.instance.selectedClass;

        if (level0Complete)
        {
            hasCarKey = false;
        }

        //Variable assignment
        cam = GetComponent<CameraController>();
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        maxHealth = PlayerData.instance.maxHealth;
        xp = PlayerData.instance.xp;
        coins = PlayerData.instance.coins;
        regen = PlayerData.instance.regen;
        poison = PlayerData.instance.poison;
        freezing = PlayerData.instance.freezing;

        xpCount.text = "XP: " + xp.ToString();
        coinCount.text = "Coins: " + coins.ToString();

        if (classType == ClassType.TANK)
        {
            maxHealth = Mathf.FloorToInt(maxHealth * 1.5f);
        }
        else if (classType == ClassType.ATHLETE)
        {
            moveSpeed *= 2f;
            jumpForce *= 2f;
        }

        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);

        ammoCounter.setAmmo(AmmoType.SMALL, smallAmmo);
        ammoCounter.setAmmo(AmmoType.MEDIUM, medAmmo);
        ammoCounter.setAmmo(AmmoType.LARGE, largeAmmo);

        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
        }

        if (!hasPotion && !isPlayerDriving)
        {
            //Get the keyboard's axis for movement
            x = Input.GetAxis("Horizontal");  //To move left and right
            z = Input.GetAxis("Vertical");    //To move foward and backward

            //If player is walking, show animation
            bool hasHorizontalInput = !Mathf.Approximately(x, 0f);
            bool hasVerticalInput = !Mathf.Approximately(z, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            animator.SetBool("isWalking", isWalking);
            audioController.PlayerMovementAudio(isWalking);
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasPotion == false)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q) && equippedWeapon != null && !hasPotion)
        {
            DropWeapon();
        }

        if (Input.GetMouseButtonDown(0) && equippedWeapon != null && !pauseMenu.activeInHierarchy && !hasPotion)
        {
            equippedWeapon.Shoot(poison, freezing);
        }

        if (regen && regenTimer < regenTime)
        {
            regenTimer += Time.deltaTime;
        }

        if (regenTimer >= regenTime)
        {
            health += 5;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            healthBar.SetHealth(health);
            regenTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!hasPotion)
        {
            //Store the players movement direction
            Vector3 move = transform.right * x + transform.forward * z;

            //Multiply the movement direction vector by the movement speed
            move *= moveSpeed;

            //Set the y value of the direction equal to the rigidbody's y value
            //For physics calculations such as gravity
            move.y = rb.velocity.y;

            //Set the direction
            rb.velocity = move;
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("isWalking", false);
            audioController.PlayerMovementAudio(false);
        }

    }

    public void ShowPauseMenu()
    {
        // Making sure this is updated
        regen = PlayerData.instance.regen;
        poison = PlayerData.instance.poison;
        freezing = PlayerData.instance.freezing;

        if (menuUp)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            //Lock the mouse cursor to the center of the game window
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pauseMenu.SetActive(true);
            pauseMenu.transform.GetChild(3).gameObject.SetActive(false);
            Time.timeScale = 0;
            //Lock the mouse cursor to the center of the game window
            Cursor.lockState = CursorLockMode.None;
        }

        menuUp = !menuUp;
        cam.togglePause();
    }

    public int checkAmmo(AmmoType ammoType)
    {
        if (ammoType == AmmoType.SMALL)
        {
            return smallAmmo;
        }
        else if (ammoType == AmmoType.MEDIUM)
        {
            return medAmmo;
        }
        else if (ammoType == AmmoType.LARGE)
        {
            return largeAmmo;
        }
        else return 0;
    }

    public void addXP(int amt)
    {
        xp += amt;
        PlayerData.instance.xp = xp;
        xpCount.text = "XP: " + xp.ToString();
    }

    public void addAmmo(AmmoType ammoType, int count)
    {
        int updatedAmmo = 0;
        if (ammoType == AmmoType.SMALL)
        {
            smallAmmo += count;
            updatedAmmo = smallAmmo;
        }
        else if (ammoType == AmmoType.MEDIUM)
        {
            medAmmo += count;
            updatedAmmo = medAmmo;
        }
        else if (ammoType == AmmoType.LARGE)
        {
            largeAmmo += count;
            updatedAmmo = largeAmmo;
        }

        ammoCounter.setAmmo(ammoType, updatedAmmo);
    }

    void Jump()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1f, groundMask);

        if (grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void PickUpWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
        audioController.PickupWeaponAudio();
    }

    public void PickUpCoin()
    {
        coins++;
        PlayerData.instance.coins = coins;
        coinCount.text = "Coins: " + coins.ToString();
        audioController.CoinAudio();
    }

    public void SpendCoins(int count)
    {
        coins -= count;
        PlayerData.instance.coins = coins;
        coinCount.text = "Coins: " + coins.ToString();
    }

    void DropWeapon()
    {
        equippedWeapon.Drop();
        equippedWeapon = null;
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            healthBar.SetHealth(health);
            audioController.PlayerDamageAudio();
        }

        if (health <= 0)
        {
            health = 0;
            healthBar.SetHealth(health);
            animator.SetTrigger("isDead");
        }
    }

    public void PickUpWpnPart()
    {
        wpnPartsCollected++;

        if (wpnPartsCollected == 2)
        {
            finalWeapon.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarKey"))
        {
            hasCarKey = true;
            audioController.PickupItemAudio();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("CityKey1"))
        {
            hasCityKey1 = true;
            audioController.PickupItemAudio();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("CityKey2"))
        {
            hasCityKey2 = true;
            audioController.PickupItemAudio();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("CityKey3"))
        {
            hasCityKey3 = true;
            audioController.PickupItemAudio();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.name.Equals("CityCheckpoint") && hasCityKey1 && hasCityKey2 && hasCityKey3)
        {
            level1Complete = true;
        }

        if (other.CompareTag("GasTank"))
        {
            hasCarKey = true;
            audioController.PickupItemAudio();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Potion"))
        {
            gameObject.GetComponent<AudioSource>().Play();
            audioController.PickupItemAudio();
            hasPotion = true;
            instance.hasPotion = true;
            other.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }
}
