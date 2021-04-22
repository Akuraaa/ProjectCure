
public abstract class ZombieState : State
{
    protected ZombieIA _zombie;
    public ZombieState(StateMachine sm, ZombieIA zombie) : base(sm)
    {
        _zombie = zombie;
    }
}
