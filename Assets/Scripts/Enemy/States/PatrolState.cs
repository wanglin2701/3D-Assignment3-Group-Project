using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;

    public override void Enter()
    {
        // Find the nearest waypoint
        waypointIndex = FindNearestWaypointIndex();

        // Set the agent's destination to the nearest waypoint
        enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);

        // Reset the walking animation
        enemy.animator.SetBool("isWalking", true); 
    }

    public override void Perform()
    {
        PatrolCycle();

        // If the enemy can see the player, switch to attack state
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }

        // Update walking animation based on movement
        if (enemy.Agent.velocity.sqrMagnitude > 0.1f) // Checks if enemy is moving
        {
            enemy.animator.SetBool("isWalking", true); // Set walking animation on
        }
        else
        {
            enemy.animator.SetBool("isWalking", false); // Set walking animation off
        }
    }

    public override void Exit()
    {
        // Stop the walking animation when exiting patrol state
        enemy.animator.SetBool("isWalking", false); 
    }

    public void PatrolCycle()
    {
        // Patrol logic
        if (enemy.Agent.remainingDistance < 0.2f) 
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > 2) 
            {
                // Move to the next waypoint in the patrol route
                waypointIndex = (waypointIndex + 1) % enemy.enemyPath.waypoints.Count;

                // Set new destination
                enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);

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
