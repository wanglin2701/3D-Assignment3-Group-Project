using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcPatrol : BaseState
{
    public int waypointIndex;
    public float waitTimer;

    public override void Enter()
    {
        // Ensure meleeEnemyPath and waypoints are valid
        if (meleeEnemy.enemyPath == null || meleeEnemy.enemyPath.waypoints == null || meleeEnemy.enemyPath.waypoints.Count == 0)
        {
            Debug.LogError("EnemyPath or waypoints are not set correctly!");
            return;
        }

        // Find the nearest waypoint
        waypointIndex = FindNearestWaypointIndex();

        // Set the agent's destination to the nearest waypoint
        if (meleeEnemy.Agent != null)
        {
            meleeEnemy.Agent.SetDestination(meleeEnemy.enemyPath.waypoints[waypointIndex].position);
        }
        else
        {
            Debug.LogError("NavMeshAgent is not assigned on the enemy!");
        }

        // Reset the walking animation
        if (meleeEnemy.animator != null)
        {
            meleeEnemy.animator.SetBool("isWalking", true);
        }
    }


    public override void Perform()
    {
        OrcPatrolCycle();

        // If the enemy can see the player, switch to attack state
        if (meleeEnemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }

        // Update walking animation based on movement
        if (meleeEnemy.Agent.velocity.sqrMagnitude > 0.1f) // Checks if enemy is moving
        {
            meleeEnemy.animator.SetBool("isWalking", true); // Set walking animation on
        }
        else
        {
            meleeEnemy.animator.SetBool("isWalking", false); // Set walking animation off
        }
    }

    public override void Exit()
    {
        // Stop the walking animation when exiting patrol state
        meleeEnemy.animator.SetBool("isWalking", false); 
    }

    public void OrcPatrolCycle()
    {
        if (meleeEnemy.Agent == null || meleeEnemy.enemyPath == null || meleeEnemy.enemyPath.waypoints.Count == 0)
        {
            Debug.LogError("NavMeshAgent or waypoints are missing. PatrolCycle cannot execute.");
            return;
        }

        // Patrol logic
        if (meleeEnemy.Agent.remainingDistance < 0.2f) 
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > 2) 
            {
                // Move to the next waypoint in the patrol route
                waypointIndex = (waypointIndex + 1) % meleeEnemy.enemyPath.waypoints.Count;

                // Set new destination
                meleeEnemy.Agent.SetDestination(meleeEnemy.enemyPath.waypoints[waypointIndex].position);

                waitTimer = 0;
            }
        }
    }

    


    private int FindNearestWaypointIndex()
    {
        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        // Loop through all waypoints to find the closest one
        for (int i = 0; i < enemy.enemyPath.waypoints.Count; i++)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.enemyPath.waypoints[i].position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }
}
