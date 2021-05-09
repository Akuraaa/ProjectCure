using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    float idleDuration = 5;
    float currentIdleDuration = 0;
    public EnemyIdleState(StateMachine sm, EnemyAI enemy) : base(sm, enemy)
    {
    }

    public override void Awake()
    {
        base.Awake();
        idleDuration = Random.Range(_enemy.minIdleTime, _enemy.maxIdleTime);
        currentIdleDuration = 0;
        _enemy.animator.enabled = false;
        _enemy.animator.SetFloat("AngularSpeed", 0);
        _enemy.animator.SetFloat("Speed", 0);
    }

    public override void Execute()
    {
        base.Execute();
        currentIdleDuration += Time.deltaTime;

        if (currentIdleDuration > idleDuration)
            _sm.SetState<EnemyPatrolState>();
    }
}
