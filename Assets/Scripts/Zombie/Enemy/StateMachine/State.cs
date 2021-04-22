using UnityEngine;
using System.Collections;

public class State {

    protected StateMachine _sm;

    public State(StateMachine sm)
    {
        _sm = sm;
    }

    public virtual void Awake() { }

    public virtual void Execute() { }

    public virtual void LateExecute() { }

    public virtual void Sleep() { }
}
