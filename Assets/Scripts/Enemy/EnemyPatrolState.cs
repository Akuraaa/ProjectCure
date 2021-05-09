using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(StateMachine sm, EnemyAI enemy) : base(sm, enemy)
    {

    }

    public override void Awake()
    {
        base.Awake();
        _enemy.animator.SetBool("IsWalking", true);
    }

    public override void Execute()
    {
        base.Execute();
        Transform target = _enemy.waypoints[_enemy.currentWaypointTarget];
        _enemy.transform.LookAt(target);

        if (Vector3.Distance(_enemy.transform.position, target.position) < .5)
        {
            if (_enemy.currentWaypointTarget < _enemy.waypoints.Count - 1)
            {
                _enemy.currentWaypointTarget++;
            }
            else
            {
                _enemy.waypoints.Reverse();
                _enemy.currentWaypointTarget = 0;
            }
        }
    }
}
