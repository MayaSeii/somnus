using UnityEngine;

public abstract class FSMAction : ScriptableObject
{
    public abstract void Execute(BaseStateMachine stateMachine);
    public virtual void Exit(BaseStateMachine stateMachine) {}
    public virtual void Enter(BaseStateMachine stateMachine) {}
}