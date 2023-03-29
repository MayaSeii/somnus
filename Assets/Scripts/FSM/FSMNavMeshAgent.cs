using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavMeshAgent : MonoBehaviour
{
    [field: SerializeField] public Transform[] patrolWaypoints;
    [field: SerializeField] public Transform target; 

    private NavMeshAgent _agent;

    void Start() { _agent = GetComponent<NavMeshAgent>(); }

    public void GoToNextPatrolWaypoint() { _agent.SetDestination(patrolWaypoints[Random.Range(0, patrolWaypoints.Length)].position); }

    public void GoToTarget(){ _agent.SetDestination(target.position); }

    public bool IsAtDestination()
    {
        if (!_agent.pathPending)
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    return true;
        return false;
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }

}
