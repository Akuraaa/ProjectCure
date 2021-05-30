using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : ZombieState
{
    public MeleeAttackState(StateMachine sm, ZombieBase z) : base(sm, z)
    {
    }

    public override void Awake()
    {
        base.Awake();
        zombie.speed = 0;
        zombie._anim.SetBool("IsAttacking", true);
    }

    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(zombie.player.transform.position, zombie.transform.position) < zombie.zombieMeleeDist)
        {
            _sm.SetState<ChaseState>();
        }
        else
        {
            zombie.target = zombie.playerPosDetection;
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        zombie._anim.SetBool("IsAttacking", false);
    }
}
