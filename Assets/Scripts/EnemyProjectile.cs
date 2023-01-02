using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to handle Enemy projectile
public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public int bulletDamage = 10;

    private GameObject playerGO;            //Variable to reference the Player GameObject
    private PlayerController player;        //Variable to reference the Player object
    private Transform playerTransform;      //Variable to reference the Player's eyes' transform component
    private Vector3 target;                 //Variable to store the target destination

    void Awake()
    {
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<PlayerController>();
        playerTransform = playerGO.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        transform.LookAt(playerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<PlayerController>())
        {
            player.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
