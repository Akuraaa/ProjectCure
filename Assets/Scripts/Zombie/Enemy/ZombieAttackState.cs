using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : ZombieState
{
    public ZombieAttackState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {

    }

    public override void Awake()
    {
        base.Awake();
        _zombie._anim.SetBool("Attack", true);
    }

    public override void Execute()
    {
        base.Execute();
        //_zombie.player.ReceiveDamage(_zombie.damage);
    }

    public override void Sleep()
    {
        base.Sleep();
        _zombie._anim.SetBool("Attack", false);
    }
}
