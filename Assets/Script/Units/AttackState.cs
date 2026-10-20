using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    private AttackStateSO config;
    private Quaternion  savedRotation;
    private float timer;
    private Coroutine attackRoutine;
    public AttackState(Unit unit, IStateMachine stateMachine, AttackStateSO config) : base(unit, stateMachine)
    {
        unit.Agent.speed = config.agentSpeed;
        this.config = config;
    }

    public override void EnterState()
    {
        timer = config.disengageTimer;
        savedRotation = unit.visualObject.transform.rotation;
        unit.Agent.stoppingDistance = Mathf.Min(unit.weapon.MaxAIRange, config.stoppingDistance);
        unit.Agent.ResetPath();
        base.EnterState();
        //I know it should be done differently but this is the quickest way to work with the current wweapon design
        if (!config.isMeleeAttacking)
        {
            unit.weapon.StartFiring();
            SoundManagerMino.Instance.PlaySound(unit.DroneShoot,unit.AudioSource);
        }
        else
        {
            attackRoutine = unit.StartCoroutine(MeleeAttack());
        }
        
    }

    public override void FrameUpdate()
    {
        unit.Agent.SetDestination(unit.target.transform.position);
        if (config.rotateTowardUnit)
        {
            Vector3 directionToTarget =
                (unit.target.transform.position - unit.visualObject.transform.position).normalized;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                unit.visualObject.transform.rotation = Quaternion.Slerp(
                    unit.visualObject.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }
            
            Vector3 directionToTargetSpawnLocation =
                (unit.target.transform.position - unit.spawnLocation.transform.position).normalized;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTargetSpawnLocation);
                unit.spawnLocation.transform.rotation = Quaternion.Slerp(
                    unit.spawnLocation.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }
        }
        if(timer >= 0) timer -= Time.deltaTime;
        base.FrameUpdate();
        CheckTransition();
    }

    public IEnumerator MeleeAttack()
    {
        unit.anim.SetTrigger("MeleeAttack");
        SoundManagerMino.Instance.PlaySound(unit.EnemyDeath,unit.AudioSource);
        yield return new WaitForSeconds(config.delayBeforeDamage);
        Collider[] results = Physics.OverlapSphere(unit.transform.position, config.radiusOfMelee, config.meleeLayer);
        foreach (Collider hit in results)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                IDamageable damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(unit.gameObject, config.meleeDamage);
                }
            }
        }
        yield return new WaitForSeconds(config.ColdownAfterMeleeAttack);
        unit.StateMachine.ChangeState(unit.chaseState);
        yield return null;
    }
    public override void ExitState()
    {
        unit.visualObject.transform.rotation = savedRotation;
        unit.weapon.StopFiring();
        if (config.isMeleeAttacking)
        {
            unit.StopCoroutine(attackRoutine);
        }
        base.ExitState();
    }

    public override void CheckTransition()
    {
        if(timer > 0) return;
        if (Vector3.Distance(unit.gameObject.transform.position, unit.target.transform.position) >= unit.weapon.MaxAIRange)
        {
            unit.StateMachine.ChangeState(unit.chaseState);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (unit == null || config == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            unit.transform.position,
            config.radiusOfMelee
        );
    }
}
