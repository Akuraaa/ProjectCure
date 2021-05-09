using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : State
{
    protected EnemyAI _enemy;
    public EnemyState(StateMachine sm, EnemyAI enemy) : base(sm)
    {
        _enemy = enemy;
    }
}
