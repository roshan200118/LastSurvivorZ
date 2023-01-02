using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for Shooting Enemy
public class ShootingEnemy : Enemy
{
    public GameObject projectile;

    public float shootCooldown = 2;
    float shootTimer = 0;

    public override void Update()
    {
        base.Update();
        shootTimer += Time.deltaTime;
    }

    public override void Attack()
    {
        transform.LookAt(playerObj.transform);

        if (shootTimer >= shootCooldown)
        {
            animator.SetTrigger("attack");
            Instantiate(projectile, transform.position + transform.up * 1.5f + transform.forward * 2f, Quaternion.identity);
            shootTimer = 0;
        }
    }
}
