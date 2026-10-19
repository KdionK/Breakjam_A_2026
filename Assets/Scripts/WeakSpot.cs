using UnityEngine;

public class WeakSpot : MonoBehaviour, IDamageable
{
    [SerializeField] private Unit unit;
    [SerializeField] private float damageMultiplier = 2f;

    void Awake()
    {
        if (unit == null) unit = GetComponentInParent<Unit>();
    }

    public DamageInfo TakeDamage(GameObject source, float damage)
    {
        return unit.TakeDamage(source, damage * damageMultiplier);
    }

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx) => ctx;
    public bool IsWounded() => unit.IsWounded();
    public HealthComponent GetHealth() => unit.GetHealth();
}