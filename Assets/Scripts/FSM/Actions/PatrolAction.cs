using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Patrol")]
//It's trying to access the class action
public class PatrolAction : Action 
{
    //We need an override so it works
    public override void Act(FiniteStateMachine fsm)
    {
        Debug.Log("Patrol");
        if(fsm.GetNavMeshAgent().IsAtDestination())
        {
            fsm.GetNavMeshAgent().GoToNextPatrolWaypoint(); 
        }
    }
}
