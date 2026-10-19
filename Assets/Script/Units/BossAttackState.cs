using System.Collections;
using UnityEngine;

public class BossAttackState : AttackState
{
    private readonly BossAttackStateSO bossConfig;
    private float phaseTimer;
    private bool isMeleeing;
    private Coroutine bossRoutine;
    private float rangedStoppingDistance;
    public Boss boss;

    public BossAttackState(Unit unit, IStateMachine stateMachine, BossAttackStateSO config)
        : base(unit, stateMachine, config)
    {
        bossConfig = config;
        boss = (Boss)unit;
    }

   

    public override void EnterState()
    {
        base.EnterState();   
        rangedStoppingDistance =  Mathf.Min(unit.weapon.MaxAIRange, bossConfig.stoppingDistance);
        phaseTimer = bossConfig.rangedDuration;
        isMeleeing = false;
        boss.weapon2.StartFiring();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (unit.StateMachine.CurrentState != this) return;

        if (isMeleeing) return;
        
        Vector3 directionToTarget =
            (unit.target.transform.position - unit.spawnLocation.transform.position).normalized;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            unit.spawnLocation.transform.rotation = Quaternion.Slerp(
                unit.spawnLocation.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
            
        Vector3 directionToTargetSpawnLocation =
            (unit.target.transform.position - boss.obj.transform.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTargetSpawnLocation);
            boss.obj.transform.rotation = Quaternion.Slerp(
                boss.obj.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
        
        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
        {
            bossRoutine = unit.StartCoroutine(StrongMelee());
        }
    }

    public override void CheckTransition()
    {
        if (isMeleeing) return; 
        base.CheckTransition();
    }

    private IEnumerator StrongMelee()
    {
        isMeleeing = true;
        unit.weapon.StopFiring();
        boss.weapon2.StopFiring();
        unit.Agent.stoppingDistance = bossConfig.meleeStoppingDistance;

        float timeout = 5f; 
        while (timeout > 0f &&
               Vector3.Distance(unit.transform.position, unit.target.transform.position)
               > bossConfig.meleeStoppingDistance + 0.3f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(bossConfig.delayBeforeDamage);

        Collider[] hits = Physics.OverlapSphere(unit.transform.position, bossConfig.radiusOfMelee, bossConfig.meleeLayer);
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Player")) continue;
            hit.GetComponent<IDamageable>()?.TakeDamage(unit.gameObject, bossConfig.meleeDamage);
        }

        yield return new WaitForSeconds(bossConfig.ColdownAfterMeleeAttack);

        unit.Agent.stoppingDistance = rangedStoppingDistance;
        unit.weapon.StartFiring();
        boss.weapon2.StartFiring();
        phaseTimer = bossConfig.rangedDuration;
        isMeleeing = false;
    }

    public override void ExitState()
    {
        if (bossRoutine != null) unit.StopCoroutine(bossRoutine);
        isMeleeing = false;
        unit.Agent.stoppingDistance = rangedStoppingDistance;
        boss.weapon2.StopFiring();
        base.ExitState();
    }
}