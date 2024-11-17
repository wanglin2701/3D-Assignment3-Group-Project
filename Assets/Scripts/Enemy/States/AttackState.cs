using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    public override void Enter()
    {
        enemy.animator.SetBool("isWalking", false);
    }

    public override void Exit()
    {
        enemy.animator.SetBool("isWalking", false);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            Vector3 directionToPlayer = (enemy.Player.transform.position - enemy.transform.position);
            directionToPlayer.y = 0;

            // Rotate to face the player
            if (directionToPlayer != Vector3.zero)
            {
                enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }

            // Shoot logic
            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            // Movement logic
            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;

                // Play walking animation if the agent is moving
                if (enemy.Agent.velocity.sqrMagnitude > 0.1f)
                {
                    enemy.animator.SetBool("isWalking", true);
                }
            }
            else
            {
                if (enemy.Agent.velocity.sqrMagnitude < 0.1f)
                {
                    enemy.animator.SetBool("isWalking", false);
                }
            }

            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 3)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        Transform gunBarrel = enemy.gunBarrel;

        // Instantiate the bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);

        // Set the attack damage on the bullet (using the unique value for this enemy)
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = enemy.attackDamage; // Set the damage of the bullet from the enemy's attackDamage
        }

        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;

        shotTimer = 0;
    }

}
