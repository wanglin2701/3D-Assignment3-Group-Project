using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;

    public override void Enter()
    {
        // Optionally reset walking state when entering patrol
        enemy.animator.SetBool("isWalking", true); // Set walking animation on
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
        // Optionally stop walking animation when exiting patrol state
        enemy.animator.SetBool("isWalking", false); // Set walking animation off
    }

    public void PatrolCycle()
    {
        // The patrol logic
        if (enemy.Agent.remainingDistance < 0.2f) // If the enemy has reached the current waypoint
        {
            waitTimer += Time.deltaTime;

            // Stop the walking animation when idle
            if (waitTimer > 2)
            {
                enemy.animator.SetBool("isWalking", false); // Stop walking animation when paused

                if (waypointIndex < enemy.enemyPath.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;

                // Set new destination for the agent
                enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
        else
        {
            // Ensure the walking animation plays only when the agent is moving
            if (enemy.Agent.velocity.sqrMagnitude > 0.1f) // Agent is moving
            {
                enemy.animator.SetBool("isWalking", true); // Set walking animation on
            }
            else
            {
                enemy.animator.SetBool("isWalking", false); // Stop walking animation if no movement
            }
        }
    }
}
