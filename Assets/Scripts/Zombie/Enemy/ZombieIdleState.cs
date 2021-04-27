//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ZombieIdleState : ZombieState
//{
//    float idleDuration = 5;
//    float currentIdleDuration = 0;
//    public ZombieIdleState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
//    {

//    }

//    public override void Awake()
//    {
//        base.Awake();
//        _zombie._anim.SetBool("Idle", true);
//    }

//    public override void Execute()
//    {
//        base.Execute();
//        currentIdleDuration += Time.deltaTime;
//        if (currentIdleDuration > idleDuration)
//            Sleep();
//    }

//    public override void Sleep()
//    {
//        base.Sleep();
//        _zombie._anim.SetBool("Idle", false);
//    }
//}
