using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : ZombieState
{
    public ChaseState(StateMachine sm, ZombieBase z): base(sm, z)
    {
    }

    public override void Awake()
    {
        base.Awake();
        zombie.currentWaypoint = 0;
        zombie.speed = 1;
        zombie._anim.SetBool("IsWalking", true);

    }

    public override void Execute()
    {
        base.Execute();
        if (zombie.currentWaypoint > zombie.waypoints.Count - 1)
            _sm.SetState<IdleState>();

        else
        {
            zombie.transform.rotation = Quaternion.LookRotation(new Vector3(zombie.player.transform.position.x, zombie.transform.position.y, zombie.player.transform.position.z) - zombie.transform.position);
            zombie.transform.position += zombie.transform.forward * zombie.speed * Time.deltaTime;
            if (zombie.isRange)
            {
                if (Vector3.Distance(zombie.transform.position, zombie.player.transform.position) > zombie.zombieRangeDist)
                {
                    _sm.SetState<RangeAttackState>();
                }
            }
            else
            {
                if (Vector3.Distance(zombie.transform.position, zombie.player.transform.position) > zombie.zombieMeleeDist)
                {
                    _sm.SetState<MeleeAttackState>();
                }
            }
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        zombie._anim.SetBool("IsWalking", false);
    }
}
