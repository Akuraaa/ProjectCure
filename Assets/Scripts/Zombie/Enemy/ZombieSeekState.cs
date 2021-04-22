using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSeekState : ZombieState
{
    Transform target;
    float obstacleAvoidanceDistance = 5;
    Vector3 avoidance = Vector3.zero;
    public ZombieSeekState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {

    }

    public override void Awake()
    {
        base.Awake();
        _zombie._anim.SetBool("Running", true);
    }

    public override void Execute()
    {
        base.Execute();
        avoidance = Vector3.zero;
        float minDistance = obstacleAvoidanceDistance;
        RaycastHit ray;
        target = _zombie.player.transform;
        _zombie.transform.LookAt(target);
        if (Physics.Raycast(_zombie.transform.position, _zombie.transform.forward, out ray, obstacleAvoidanceDistance))
        {
            for (int i = 10; i <= 180; i += 10)
            {
                bool collide = false;
                if (Physics.Raycast(_zombie.transform.position, Quaternion.AngleAxis(i, Vector3.up) * _zombie.transform.forward, out ray, obstacleAvoidanceDistance))
                    collide = true;
                else
                    avoidance = Quaternion.AngleAxis(i, Vector3.up) * _zombie.transform.forward;

                if (Physics.Raycast(_zombie.transform.position, Quaternion.AngleAxis(-i, Vector3.up) * _zombie.transform.forward, out ray, obstacleAvoidanceDistance))
                    collide = true;
                else
                    avoidance = Quaternion.AngleAxis(-i, Vector3.up) * _zombie.transform.forward;
                if (collide) continue;
                break;
            }
        }
        float speedMulti = minDistance / obstacleAvoidanceDistance;
        avoidance.Normalize();
    }

    public override void Sleep()
    {
        base.Sleep();
        _zombie._anim.SetBool("Running", false);
    }
}
