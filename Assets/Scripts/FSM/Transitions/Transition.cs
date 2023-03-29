using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Transition")]
public class Transition : ScriptableObject
{
    [SerializeField] private Condition _decision;
    [SerializeField] private Action _action;
    [SerializeField] private State _targetState;

    public bool IsTriggered(FiniteStateMachine fsm) { return _decision.Test(fsm); }

    public Action GetAction() { return _action; }

    public State GetTargetState() { return _targetState; }
}