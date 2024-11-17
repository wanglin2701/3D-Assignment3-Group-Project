using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAttack : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float attackTimer;  // To track when to deal damage
    public PlayerHealth playerHealth;
    public float damage = 20;

    public override void Enter()
    {
        // Stop walking animation when attacking
        meleeEnemy.animator.SetBool("isWalking", false);
    }

    public override void Exit()
    {
        // Optionally reset walking state when exiting the attack state
        meleeEnemy.animator.SetBool("isWalking", false);
    }

    public override void Perform()
    {
        if (meleeEnemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;  // Increment attack timer

            Vector3 directionToPlayer = (meleeEnemy.Player.transform.position - meleeEnemy.transform.position);
            directionToPlayer.y = 0;

            // Rotate to face the player
            if (directionToPlayer != Vector3.zero)
            {
                meleeEnemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }

            // Attack logic: damage player if the orc gets close
            if (attackTimer > 1f)  // Attack every 1 second or adjust as necessary
            {
                AttackPlayer();
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

    // Function to deal damage to the player when in range
    private void AttackPlayer()
    {
        float attackRange = 2f; // Adjust as needed for the orc's attack range

        // Check distance between orc and player
        float distanceToPlayer = Vector3.Distance(meleeEnemy.transform.position, meleeEnemy.Player.transform.position);

        if (distanceToPlayer <= attackRange)  // If the orc is close enough
        {
            Debug.Log("Orc is attacking the player!");

            // Call function to deal damage to the player
            playerHealth.TakeDamage(damage);  // Assuming Player has a TakeDamage method

            // Reset the attack timer
            attackTimer = 0;
        }
    }
}
