using UnityEngine;

public class ChaseState : State
{
    private ChaseStateSO config;
    private bool hasTriggeredAttack = false;
    private readonly float normalStoppingDistance;
    private bool hasLineOfSight;
    private static readonly RaycastHit[] losHits = new RaycastHit[16];

    public ChaseState(Unit unit, IStateMachine stateMachine, ChaseStateSO config) : base(unit, stateMachine)
    {
        this.config = config;
        normalStoppingDistance = unit.Agent.stoppingDistance;
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
            hasLineOfSight = CheckLineOfSight();
            unit.Agent.stoppingDistance = hasLineOfSight ? normalStoppingDistance : 0f;

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
        CheckTransition();
    }

    public override void CheckTransition()
    {
        if (!hasLineOfSight) return;

        float range = config.UseMeleeWeapon ? config.DistanceMeleeToTurnAttack : unit.weapon.MaxAIRange;
        if (Vector3.Distance(unit.transform.position, unit.target.transform.position) <= range)
        {
            unit.StateMachine.ChangeState(unit.attackState);
        }
    }

    private bool CheckLineOfSight()
    {
        Vector3 origin = unit.transform.position + Vector3.up * 1.5f;
        Vector3 end = unit.target.transform.position + Vector3.up * 1f;
        Vector3 dir = end - origin;
        float dist = dir.magnitude;

        int count = Physics.RaycastNonAlloc(origin, dir / dist, losHits, dist,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            Transform t = losHits[i].transform;
            if (t.IsChildOf(unit.transform) || t.IsChildOf(unit.target.transform)) continue; 
            return false;
        }
        return true;
    }
}