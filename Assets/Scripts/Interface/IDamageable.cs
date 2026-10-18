using UnityEngine;

public interface IDamageSource
{
    string DisplayName { get; } 
    GameObject SourceActor { get; }
}

public class DamageProposalContext
{
    public GameObject Self;
    public GameObject Source;
    public float DamageProposed;
}

public class DamageMitigationContext
{
    public GameObject Self;
    public GameObject Source;

    public float IncomingDamage;
    public float Defense;
}
public struct DamageInfo
{
    public GameObject Source;
    public GameObject Target;
    public IDamageSource damageSource;
    public float finalDamage;

    public DamageInfo(GameObject source, GameObject target, IDamageSource damageSource, float finalDamage)
    {
        Source = source;
        Target = target;
        this.damageSource = damageSource;
        this.finalDamage = finalDamage;
    }
}


public interface IDamageable
{
    public DamageInfo TakeDamage(GameObject source, IDamageSource damageSource, float damage);
    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx);

    public bool IsWounded();
    
    public HealthComponent GetHealth();
}
