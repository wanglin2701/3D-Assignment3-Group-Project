using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcSearch : BaseState
{
    private float moveTimer;    // Timer to control random movement
    private float searchTimer;  // Timer to determine when to switch back to patrol
    private float originalSpeed;

    public override void Enter()
    {
        originalSpeed = enemy.Agent.speed;
        meleeEnemy.Agent.speed *= 1.5f; // Increase speed during searching
        meleeEnemy.Agent.SetDestination(enemy.LastKnowPos);

        meleeEnemy.animator.SetBool("isWalking", true); // Trigger walking animation
        searchTimer = 0; // Reset search timer on entry
        moveTimer = 0;   // Reset move timer on entry
    }

    public override void Perform()
    {
        // Check if the player is visible again
        if (meleeEnemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }

        // Increment timers
        searchTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        // Move to a random position if enough time has passed
        if (moveTimer > Random.Range(2f, 5f))
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-10f, 10f),
                0,
                Random.Range(-10f, 10f)
            );
            Vector3 newDestination = meleeEnemy.transform.position + randomDirection;

            if (NavMesh.SamplePosition(newDestination, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                meleeEnemy.Agent.SetDestination(hit.position);
            }
            else
            {
                Debug.Log("Failed to find a valid position to move to.");
            }

            moveTimer = 0; // Reset the movement timer
        }

        // Check if enough time has passed to return to patrol state
        if (searchTimer > 10f) // Search for 10 seconds before returning to patrol
        {
            Debug.Log("Search timer exceeded. Returning to patrol.");
            stateMachine.ChangeState(new PatrolState());
        }

        // Update walking animation based on movement
        meleeEnemy.animator.SetBool("isWalking", meleeEnemy.Agent.velocity.sqrMagnitude > 0.1f);
    }

    public override void Exit()
    {
        meleeEnemy.Agent.speed = originalSpeed; // Reset speed to normal
        meleeEnemy.animator.SetBool("isWalking", false); // Stop walking animation
    }
}
