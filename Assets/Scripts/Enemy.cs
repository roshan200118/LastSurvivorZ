using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject playerObj, coinPrefab, smallAmmoPrefab, medAmmoPrefab;
    public LayerMask playerMask;
    public Animator animator;
    public AudioController audioController;
    public GameObject potion;           //Used by Boss enemy

    private Vector3 lastPosition;
    private GameObject ice;

    public float detectRange = 10f;
    public float attackRange = 1f;
    public float attackSpeed = 2f;
    public int health = 50;
    public int strength = 5;
    public int xpValue = 1;

    float attackTimer = 0;
    float poisonTimer = 0;
    float frozenTimer = 0;
    float poisonDamageTime = 2;
    float frozenTime = 20f;
    bool inDetectRange, inAttackRange;
    bool poisoned = false;
    bool frozen = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        //Assign player GO and get player's last position
        playerObj = GameObject.Find("Player");
        lastPosition = gameObject.transform.position;

        //Assign audio controller
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();

        ice = gameObject.transform.Find("Ice").gameObject;
        ice.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        inDetectRange = Physics.CheckSphere(transform.position, detectRange, playerMask);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        attackTimer += Time.deltaTime;

        //Chase player if is in range and not frozen
        if (inDetectRange && !frozen) Chase();

        //Set walking animation
        if (gameObject.transform.position != lastPosition)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        //Assign last position
        lastPosition = gameObject.transform.position;

        //If player is in attack range and enemy can attack again
        if (inAttackRange && attackTimer >= attackSpeed) Attack();

        //If enemy is poisoned
        if (poisoned)
        {
            poisonTimer += Time.deltaTime;
        }

        //Take poison damage
        if (poisonTimer >= poisonDamageTime)
        {
            takeDamage(5);
            poisonTimer = 0;
        }

        if (frozen)
        {
            ice.SetActive(true);
            frozenTimer += Time.deltaTime;

            //Stay frozen for some time
            if (frozenTimer >= frozenTime)
            {
                frozen = false;
                frozenTimer = 0;
                ice.SetActive(false);
            }
        }
    }

    //Method to take damage from Player
    public void takeDamage(int damage)
    {
        health -= damage;

        //If Enemy has no health
        if (health <= 0)
        {
            //If Boss Enemy is defeated, instantiate the potion
            if (transform.CompareTag("BossEnemy"))
            {
                Instantiate(potion, transform.position, Quaternion.identity);
            }

            //If not Boss Enemy
            else
            {
                //Add XP to player
                PlayerController.instance.addXP(xpValue);

                //0 = coin, 1 = ammo
                float randomNum = Random.Range(0, 2);

                //If level 1 is done, instantiate medium size ammo
                if (PlayerController.level0Complete)
                {
                    if (randomNum == 0)
                    {
                        Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(medAmmoPrefab, transform.position, Quaternion.identity);
                    }
                }

                //If level 0, instantiate small size ammo
                else
                {
                    if (randomNum == 0)
                    {
                        Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(smallAmmoPrefab, transform.position, Quaternion.identity);
                    }
                }
            }

            //Destory the enemy GO
            Destroy(gameObject);
        }
    }

    public void Poison()
    {
        poisoned = true;
    }

    public virtual void Freeze()
    {
        frozen = true;
    }

    public virtual void Attack()
    {
        attackTimer = 0;
        animator.SetTrigger("attack");
        audioController.ZombieAttackAudio();
        PlayerController.instance.TakeDamage(strength);
    }

    public virtual void Chase()
    {
        agent.SetDestination(playerObj.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
