using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : EnemyState
{
    public EnemyShootState(StateMachine sm, EnemyAI enemy) : base(sm, enemy)
    {
    }

    public override void Awake()
    {
        base.Awake();
        _enemy.animator.SetBool("PlayerInSight", true);
        _enemy.animator.SetFloat("Speed", 0);
        _enemy.animator.SetFloat("AngularSpeed", 0);
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 dirToPlayer = (_enemy.player.transform.position - _enemy.transform.position).normalized;
        float angle =
            Vector3.SignedAngle(
                Vector3.ProjectOnPlane(dirToPlayer, Vector3.up),
                Vector3.ProjectOnPlane(_enemy.transform.forward, Vector3.up),
                Vector3.up);
        _enemy.animator.SetFloat("AngularSpeed", -angle / 3);

    }

    public override void Sleep()
    {
        base.Sleep();
        _enemy.animator.SetBool("PlayerInSight", false);
    }
}
