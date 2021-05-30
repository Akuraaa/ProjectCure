using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ZombieState
{
    private float timeToIdle = 5;
    private float currentIdleDuration = 0;

    public IdleState(StateMachine sm, ZombieBase z) : base(sm, z)
    {
    }

    public override void Awake()
    {
        base.Awake();
        zombie._anim.SetBool("IsIdle", true);
    }
    public override void Execute()
    {
        base.Execute();
        timeToIdle = Random.Range(zombie.minIdleTime, zombie.maxIdleTime);
        currentIdleDuration = 0;
        //zombie.speed = 0;
        currentIdleDuration += Time.deltaTime;
        if (currentIdleDuration < timeToIdle)
        {
            _sm.SetState<PatrolState>();
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        zombie._anim.SetBool("IsIdle", false);
    }
}
