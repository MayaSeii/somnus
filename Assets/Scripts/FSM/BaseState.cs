using UnityEngine;

public class BaseState : ScriptableObject
{
    public virtual void Execute(BaseStateMachine machine) { }
    public virtual void Exit(BaseStateMachine machine) { }
    public virtual void Enter(BaseStateMachine machine) { }
}
