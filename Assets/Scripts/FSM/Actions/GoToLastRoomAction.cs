using System.Linq;
using Controllers;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Go To Last Room")]
public class GoToLastRoomAction : FSMAction
{
    private Transform _waypoint;

    public override void Exit(BaseStateMachine stateMachine)
    {
        _waypoint = null;
    }
    
    public override void Enter(BaseStateMachine stateMachine)
    {
        _waypoint = null;
    }
    
    public override void Execute(BaseStateMachine stateMachine)
    {
        if (!_waypoint)
        {
            var waypoints = GameObject.FindGameObjectsWithTag("Patrol Point");
            var room = FindObjectsOfType<RoomController>().Where(r => r.PlayerInRoom).ToList()[0];
            _waypoint = waypoints.First(w => w.name == room.RoomName).transform;
        }
        
        var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 5f;
        navMeshAgent.SetDestination(_waypoint.position);
    }
}
