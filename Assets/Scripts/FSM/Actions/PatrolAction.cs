using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Patrol")]
public class PatrolAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        var patrolPoints = stateMachine.GetComponent<PatrolPoints>();
        
        navMeshAgent.speed = 1f;

        if (navMeshAgent.pathPending) return;
        if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return;
        
        if (!navMeshAgent.hasPath || Mathf.Abs(navMeshAgent.velocity.sqrMagnitude) < .5f)
            navMeshAgent.SetDestination(patrolPoints.GetNext().position);
    }
}
