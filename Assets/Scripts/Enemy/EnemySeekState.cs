using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekState : EnemyState
{
    Transform  target;
    float obstacleAvoidanceDistance = 5;
    Vector3 avoidance = Vector3.zero;
    public EnemySeekState(StateMachine sm, EnemyAI enemy) : base(sm, enemy)
    {
    }

    public override void Awake()
    {
        base.Awake();
        _enemy.animator.SetFloat("AngularSpeed", 0);
    }

    public override void Execute()
    {
        base.Execute();
        _enemy.animator.SetFloat("Speed", _enemy.speed* 40);
        avoidance = Vector3.zero;
        float minDistance = obstacleAvoidanceDistance;
        RaycastHit ray;
        target = _enemy.player.transform;
        _enemy.transform.LookAt(target);
        if (Physics.Raycast(_enemy.transform.position, _enemy.transform.forward, out ray, obstacleAvoidanceDistance))
        {
            for (int i = 10; i <= 180; i += 10)
            {
                bool collide = false;
                if (Physics.Raycast(_enemy.transform.position, Quaternion.AngleAxis(i, Vector3.up) * _enemy.transform.forward, out ray, obstacleAvoidanceDistance))
                    collide = true;
                else
                    avoidance = Quaternion.AngleAxis(i, Vector3.up) * _enemy.transform.forward;

                if (Physics.Raycast(_enemy.transform.position, Quaternion.AngleAxis(-i, Vector3.up) * _enemy.transform.forward, out ray, obstacleAvoidanceDistance))
                    collide = true;
                else
                    avoidance = Quaternion.AngleAxis(-i, Vector3.up) * _enemy.transform.forward;
                if (collide) continue;
                break;
            }
        }
        float speedMulti = minDistance / obstacleAvoidanceDistance;
        avoidance.Normalize();
    }
}
