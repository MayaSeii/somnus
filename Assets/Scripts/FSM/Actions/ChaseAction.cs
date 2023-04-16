using General;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Chase")]
public class ChaseAction : FSMAction
{
    [field: SerializeField] public float Speed { get; set; }
    
    public override void Execute(BaseStateMachine stateMachine)
    {
        var navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = Speed;
        navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
    }
}
