using UnityEngine;

public class Boss : Unit
{
    public BaseWeapon weapon2;
    public GameObject obj;
    protected override AttackState CreateAttackState() => new BossAttackState(this, StateMachine, (BossAttackStateSO)attackStateSO);
}
