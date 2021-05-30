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

        //Transform target = zombie.playerPosDetection;
        //zombie.direction = (target.position - zombie.transform.position).normalized;
        //zombie.rotGoal = Quaternion.LookRotation(zombie.direction);
        //zombie.rotGoal.y = 0;
        //zombie.transform.rotation = Quaternion.Slerp(zombie.transform.rotation, zombie.rotGoal, zombie.turnSpeed);
        //zombie.transform.position += zombie.transform.forward * zombie.speed * Time.deltaTime;
        zombie.target = zombie.playerPosDetection;
    }

    public override void Sleep()
    {
        base.Sleep();
        zombie._anim.SetBool("IsWalking", false);
    }
}
