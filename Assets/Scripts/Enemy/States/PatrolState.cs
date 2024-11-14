using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PatrolState : BaseState
{
    //track which waypoint currently targeting
    public int waypointIndex;
    public float waitTimer;
    public override void Enter()
    {
        
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        
    }

    public void PatrolCycle()
    {
        //the patrol logic
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > 2)
            {
                if(waypointIndex < enemy.enemyPath.waypoints.Count - 1)
                waypointIndex++;
                
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
            
        }
    }
}
