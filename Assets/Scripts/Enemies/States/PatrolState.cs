using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : ZombieState
{
    public PatrolState(StateMachine sm, ZombieBase z): base(sm, z)
    { 
    }

    public override void Awake()
    {
        base.Awake();
        zombie.currentWaypoint = 0;
        zombie.speed = .5f;
    }

    public override void Execute()
    {
        base.Execute();
        //Transform target = zombie.waypoints[zombie.currentWaypoint].GetComponent<Transform>();
        //zombie.direction = (target.position - zombie.transform.position).normalized;
        //zombie.rotGoal.y = 0;
        //zombie.rotGoal = Quaternion.LookRotation(zombie.direction);
        //zombie.transform.rotation = Quaternion.Slerp(zombie.transform.rotation, zombie.rotGoal, zombie.turnSpeed);
        //zombie.transform.position += zombie.transform.forward * zombie.speed * Time.deltaTime;
        zombie.target = zombie.waypoints[zombie.currentWaypoint].GetComponent<Transform>();
        if (Vector3.Distance (zombie.transform.position, zombie.waypoints[zombie.currentWaypoint].transform.position) < .5f)
        {
            if (zombie.currentWaypoint < zombie.waypoints.Count -1)
            {
                zombie.currentWaypoint++;
            }
            else
            {
                zombie.waypoints.Reverse();
                zombie.currentWaypoint = 0;
            }
        }

        if (zombie.playerInSight)
        {
            _sm.SetState<ChaseState>();
        }
    }
}
