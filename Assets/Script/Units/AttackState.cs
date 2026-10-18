using UnityEngine;

public class AttackState : State
{
    private AttackStateSO config;
    
    public AttackState(Unit unit, IStateMachine stateMachine, AttackStateSO config) : base(unit, stateMachine)
    {
        unit.Agent.speed = config.agentSpeed;
        this.config = config;
    }
    
}
