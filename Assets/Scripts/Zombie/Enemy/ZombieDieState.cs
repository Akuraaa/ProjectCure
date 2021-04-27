using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDieState : ZombieState
{

    public ZombieDieState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {

    }

    public override void Awake()
    {
        base.Awake();
        _zombie.speed = 0;
        _zombie._anim.SetTrigger("Die");
        //_zombie.die = true;
    }
}
