using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/In Line Of Sight")]
public class InLineOfSightDecision : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        var enemyInLineOfSight = stateMachine.GetComponent<SightSensor>();
        return enemyInLineOfSight.IsInSight(GameManager.Instance.Player.gameObject);
    }
}
