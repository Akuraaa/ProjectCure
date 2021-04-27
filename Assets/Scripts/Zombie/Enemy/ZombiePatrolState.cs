using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePatrolState : ZombieState
{
    public ZombiePatrolState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {

    }

    public override void Awake()
    {
        base.Awake();
        _zombie._anim.SetBool("Walking", true);
    }

    public override void Execute()
    {
        base.Execute();
        Transform target = _zombie.waypoints[_zombie.currentWaypointTarget];
        _zombie.transform.LookAt(target);
        _zombie.transform.position += _zombie.transform.forward * _zombie.speed * Time.deltaTime;

        if (Vector3.Distance(_zombie.transform.position, target.position) < .5)
        {
            if (_zombie.currentWaypointTarget < _zombie.waypoints.Count - 1)
            {
                _zombie.currentWaypointTarget++;
            }
            else
            {
                _zombie.waypoints.Reverse();
                _zombie.currentWaypointTarget = 0;
            }
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        _zombie._anim.SetBool("Walking", true);
    }
}
