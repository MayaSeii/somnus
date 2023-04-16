using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transition")]
public sealed class Transition : ScriptableObject
{
    [field: SerializeField] public Decision Decision { get; set; }
    [field: SerializeField] public BaseState TrueState { get; set; }
    [field: SerializeField] public BaseState FalseState { get; set; }

    public void Execute(BaseStateMachine stateMachine)
    {
        if (Decision.Decide(stateMachine) && TrueState is not RemainInState)
        {
            stateMachine.CurrentState.Exit(stateMachine);
            stateMachine.CurrentState = TrueState;
            stateMachine.CurrentState.Enter(stateMachine);
        }
        
        else if (FalseState is not RemainInState)
            stateMachine.CurrentState = FalseState;
    }
}