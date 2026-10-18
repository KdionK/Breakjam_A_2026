using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    private AttackStateSO config;
    
    public AttackState(Unit unit, IStateMachine stateMachine, AttackStateSO config) : base(unit, stateMachine)
    {
        unit.Agent.speed = config.agentSpeed;
        this.config = config;
    }

    public override void EnterState()
    {
        unit.Agent.stoppingDistance = Mathf.Min(unit.weapon.MaxAIRange, config.stoppingDistance);
        base.EnterState();
        unit.weapon.StartFiring();
    }

    public override void FrameUpdate()
    {
        
        base.FrameUpdate();
    }
}
