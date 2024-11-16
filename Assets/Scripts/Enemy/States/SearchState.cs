using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float moveTimer;
    private float searchTimer;
    private float originalSpeed;

    public override void Enter()
    {
        originalSpeed = enemy.Agent.speed;
        enemy.Agent.speed *= 1.5f; // Increase speed for searching
        enemy.Agent.SetDestination(enemy.LastKnowPos);

        // Ensure walking animation is triggered
        enemy.animator.SetBool("isWalking", true);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }

        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }

            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }

        // Update walking animation based on movement
        if (enemy.Agent.velocity.sqrMagnitude > 0.1f)
        {
            enemy.animator.SetBool("isWalking", true);
        }
        else
        {
            enemy.animator.SetBool("isWalking", false);
        }
    }

    public override void Exit()
    {
        enemy.Agent.speed = originalSpeed; // Reset speed
        enemy.animator.SetBool("isWalking", false); // Stop walking animation
    }
}
