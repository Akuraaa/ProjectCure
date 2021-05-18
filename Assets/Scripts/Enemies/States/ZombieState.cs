using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState : State
{
    protected ZombieBase zombie;

    public ZombieState(StateMachine sm, ZombieBase z) : base (sm)
    {
        zombie = z;
    }
}
