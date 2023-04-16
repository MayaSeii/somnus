using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State")]
public class State : BaseState
{
    [field: SerializeField] public List<FSMAction> Action { get; set; }
    [field: SerializeField] public List<Transition> Transitions { get; set; }

    public override void Execute(BaseStateMachine machine)
    {
        foreach (var action in Action)
            action.Execute(machine);

        foreach (var transition in Transitions)
            transition.Execute(machine);
    }

    public override void Exit(BaseStateMachine machine)
    {
        foreach (var action in Action)
            action.Exit(machine);
    }
    public override void Enter(BaseStateMachine machine)
    {
        foreach (var action in Action)
            action.Enter(machine);
    }
}