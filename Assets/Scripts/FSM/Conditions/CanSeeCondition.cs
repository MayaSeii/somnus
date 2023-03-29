using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/CanSeeCondition")]
public class CanSeeCondition : Condition
{
    [field: SerializeField] private bool _negation;
    [field: SerializeField] private float _viewAngle;
    [field: SerializeField] private float _viewDistance;

    public override bool Test(FiniteStateMachine fsm)
    {
        Vector3 direction = fsm.GetNavMeshAgent().target.position - fsm.GetNavMeshAgent().transform.position;
        if (direction.magnitude < _viewDistance)
        {
            float angle = Vector3.Angle(direction.normalized, fsm.GetNavMeshAgent().transform.forward);
            if (angle < _viewAngle) return !_negation;
        }
        return _negation;
    }
}
