using System.Collections;
using UnityEngine;

public class BossAttackState : AttackState
{
    private readonly BossAttackStateSO bossConfig;
    private float phaseTimer;
    private bool isMeleeing;
    private Coroutine bossRoutine;
    private float rangedStoppingDistance;

    public BossAttackState(Unit unit, IStateMachine stateMachine, BossAttackStateSO config)
        : base(unit, stateMachine, config)
    {
        bossConfig = config;
    }

   

    public override void EnterState()
    {
        base.EnterState();   
        rangedStoppingDistance =  Mathf.Min(unit.weapon.MaxAIRange, bossConfig.stoppingDistance);
        phaseTimer = bossConfig.rangedDuration;
        isMeleeing = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (unit.StateMachine.CurrentState != this) return;

        if (isMeleeing) return;

        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
            bossRoutine = unit.StartCoroutine(StrongMelee());
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

        // Base FrameUpdate keeps chasing using this stopping distance
        unit.Agent.stoppingDistance = bossConfig.meleeStoppingDistance;

        float timeout = 5f; // safety if the player is unreachable
        while (timeout > 0f &&
               Vector3.Distance(unit.transform.position, unit.target.transform.position)
               > bossConfig.meleeStoppingDistance + 0.3f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        //unit.anim.SetTrigger("MeleeAttack");
        yield return new WaitForSeconds(bossConfig.delayBeforeDamage);

        Collider[] hits = Physics.OverlapSphere(unit.transform.position, bossConfig.radiusOfMelee, bossConfig.meleeLayer);
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Player")) continue;
            hit.GetComponent<IDamageable>()?.TakeDamage(unit.gameObject, bossConfig.meleeDamage);
        }

        yield return new WaitForSeconds(bossConfig.ColdownAfterMeleeAttack);

        unit.Agent.stoppingDistance = rangedStoppingDistance; // back to ranged
        unit.weapon.StartFiring();
        phaseTimer = bossConfig.rangedDuration;
        isMeleeing = false;
    }

    public override void ExitState()
    {
        if (bossRoutine != null) unit.StopCoroutine(bossRoutine);
        isMeleeing = false;
        unit.Agent.stoppingDistance = rangedStoppingDistance;
        base.ExitState();
    }
}