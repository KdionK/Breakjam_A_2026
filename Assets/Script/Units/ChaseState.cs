using UnityEngine;

public class ChaseState : State
{
    private ChaseStateSO config;
    private bool hasTriggeredAttack = false;
    
    public ChaseState(Unit unit, IStateMachine stateMachine, ChaseStateSO config) : base(unit, stateMachine)
    {
        this.config = config;
    }

    public override void EnterState()
    {
        unit.Agent.speed = config.agentSpeed;
        hasTriggeredAttack = false;
        base.EnterState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if (hasTriggeredAttack) return;

        if (unit.target != null)
        {
            unit.Agent.SetDestination(unit.target.transform.position);

            Vector3 directionToTarget =
                (unit.target.transform.position - unit.transform.position).normalized;
            directionToTarget.y = 0;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                unit.transform.rotation = Quaternion.Slerp(
                    unit.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }
        }
        
        //CheckTransition();
    }

    public override void CheckTransition()
    {
        if (Vector3.Distance(unit.gameObject.transform.position, unit.target.transform.position) <= 10)
        {
            unit.StateMachine.ChangeState(unit.attackState);
        }
    }
}
