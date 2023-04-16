using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Hasn't Seen Player")]
public class HasntSeenPlayerDecision : Decision
{
    [field: SerializeField] public float TimeToCount { get; set; }
    
    public override bool Decide(BaseStateMachine stateMachine)
    {
        var enemyInLineOfSight = stateMachine.GetComponent<SightSensor>();
        return enemyInLineOfSight.TimeWithoutSeeingPlayer >= TimeToCount;
    }
}
