using UnityEngine;

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
    public float finalDamage;

    public DamageInfo(GameObject source, GameObject target, float finalDamage)
    {
        Source = source;
        Target = target;
        this.finalDamage = finalDamage;
    }
}


public interface IDamageable
{
    public DamageInfo TakeDamage(GameObject source, float damage);
    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx);
    
    public HealthComponent GetHealth();
}
