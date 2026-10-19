using UnityEngine;

public class Boss : Unit
{
    protected override AttackState CreateAttackState() => new BossAttackState(this, StateMachine, (BossAttackStateSO)attackStateSO);
}
