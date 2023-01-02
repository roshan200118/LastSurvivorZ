using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class to control the car
public class CarController : MonoBehaviour
{
    GameObject playerObj;
    public Transform carCamAnchor;
    public Transform carExitLocation;
    public GameObject instructions;
    public LayerMask playerMask;
    public GameObject pauseMenu;

    private AudioController audioController;
    private Rigidbody rbCar;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    private bool regen, poison, freezing;       // Skill tree skills
    bool playerInRange = false;
    bool menuUp = false;
    bool playLevelEnding = false;

    public float enterCarRange = 3f;

    //Used to fade completetion screen after game done
    public CanvasGroup exitBackgroundImageCanvasGroup;

    //Used to handle fade effects
    public float fadeDuration = 1.5f;
    public float displayImageDuration = 1.5f;

    //Timer for fade effects
    float timer;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;


    private void Start()
    {
        playerObj = GameObject.Find("Player");
        rbCar = GetComponent<Rigidbody>();
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    private void FixedUpdate()
    {
        if (PlayerController.instance.isPlayerDriving)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            CarNoTilt();
        }
    }

    private void Update()
    {
        //If player is not driving, check if car is near
        if (!PlayerController.instance.isPlayerDriving)
        {
            CheckNearCar();
        }

        //If player is driving
        else
        {
            instructions.GetComponent<TextMeshProUGUI>().text = "Press R to exit car\nPress SpaceBar to brake";
            instructions.SetActive(true);
        }

        //If player is driving and wants to exit
        if (Input.GetKeyDown(KeyCode.R) && PlayerController.instance.isPlayerDriving)
        {
            playerObj.transform.position = carExitLocation.transform.position;
            playerObj.SetActive(true);
            carCamAnchor.gameObject.SetActive(false);
            PlayerController.instance.isPlayerDriving = false;
            audioController.CarAudio(false);
        }

        //If player is driving and wants to see pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerController.instance.isPlayerDriving)
        {
            ShowPauseMenu();
        }

        if (playLevelEnding)
        {
            ShowLevel0WinScreenFade();
        }
    }

    //Method to show pause menu
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
    }

    //Method to check if near the car
    private void CheckNearCar()
    {
        playerInRange = Physics.CheckSphere(transform.position, enterCarRange, playerMask);

        if (playerInRange && !PlayerController.instance.isPlayerDriving && PlayerController.hasCarKey)
        {
            instructions.SetActive(true);
            instructions.GetComponent<TextMeshProUGUI>().text = "Press E to enter car";
        }
        else
        {
            instructions.SetActive(false);
        }

        //Player can only enter if has car key
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !PlayerController.instance.isPlayerDriving && PlayerController.hasCarKey)
        {
            carCamAnchor.gameObject.SetActive(true);
            PlayerController.instance.isPlayerDriving = true;
            instructions.SetActive(false);
            playerObj.SetActive(false);
            audioController.CarAudio(true);
            audioController.PlayerMovementAudio(false);
        }
    }

    //Method to limit speed so car doesn't tilt over
    private void CarNoTilt()
    {
        float y = rbCar.velocity.y;
        float x = rbCar.velocity.x;
        float z = rbCar.velocity.z;

        if (y > 0)
        {
            Vector3 newVector = new Vector3(x, 0, z);
            rbCar.velocity = newVector;
        }
    }

    //Get the user's input
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && PlayerController.instance.isPlayerDriving)
        {
            audioController.CarHitZombieAudio();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("FirstLevelCheckpoint"))
        {
            playLevelEnding = true;
            ShowLevel0WinScreenFade();
        }
    }

    //Method when Player wins the game, fade out and show completion screen
    void ShowLevel0WinScreenFade()
    {
        timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = timer / fadeDuration;
        if (timer > fadeDuration + displayImageDuration)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerController.level0Complete = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enterCarRange);
    }
}
