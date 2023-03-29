using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [field: SerializeField] public State initialState;

    private State _currentState;
    private FSMNavMeshAgent _navMeshAgent;

    void Start()
    {
        _currentState = initialState;
        _navMeshAgent = GetComponent<FSMNavMeshAgent>();
    }

    void Update()
    {
        Transition triggeredTransition = null;

        foreach(Transition t in _currentState.GetTransitions())
            if (t.IsTriggered(this))
            {
                triggeredTransition = t;
                break;
            }

        List<Action> actions = new List<Action>();

        if (triggeredTransition != null)
        {
            actions.Add(_currentState.GetExitAction());
            actions.Add(triggeredTransition.GetAction());
            actions.Add(triggeredTransition.GetTargetState().GetEntryAction());
            _currentState = triggeredTransition.GetTargetState();
        }
        else
            foreach(Action action in _currentState.GetStateActions()) 
                actions.Add(action);

        DoActions(actions);
    }

    void DoActions(List<Action> actions)
    {
        foreach (Action action in actions)
            if (action != null)
                action.Act(this);
    }

    public FSMNavMeshAgent GetNavMeshAgent() { return _navMeshAgent; }

}

