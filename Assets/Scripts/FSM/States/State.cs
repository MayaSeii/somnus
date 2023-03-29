using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/State")]
public class State : ScriptableObject
{
    [field: SerializeField] private Action _entryAction;
    [field: SerializeField] private Action[] _stateActions;
    [field: SerializeField] private Action _exitAction;
    [field: SerializeField] private Transition[] _transitions;

    public Action GetEntryAction() { return _entryAction; }

    public Action[] GetStateActions() { return _stateActions; }

    public Action GetExitAction() { return _exitAction; }

    public Transition[] GetTransitions() { return _transitions; }
}
