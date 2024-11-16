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
        // Stop walking animation when attacking
        enemy.animator.SetBool("isWalking", false);
    }

    public override void Exit()
    {
        // Optionally reset walking state when exiting the attack state
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

            // Movement logic (if the enemy is moving)
            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;

                // Play walking animation if the agent is moving
                if (enemy.Agent.velocity.sqrMagnitude > 0.1f)
                {
                    enemy.animator.SetBool("isWalking", true); // Play walking animation
                }
            }
            else
            {
                // Ensure walking animation stops if standing still
                if (enemy.Agent.velocity.sqrMagnitude < 0.1f)
                {
                    enemy.animator.SetBool("isWalking", false); // Stop walking animation
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
        // Store reference to the gun barrel
        Transform gunBarrel = enemy.gunBarrel;

        // Instantiate a new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);

        // Calculate the direction to the player
        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;

        // Add force to the rigidbody of the bullet
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;
        Debug.Log("Shoot");
        shotTimer = 0;
    }
}
