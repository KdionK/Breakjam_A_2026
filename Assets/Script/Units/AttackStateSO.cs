using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Attack State")]
public class AttackStateSO : StateScriptableObject
{
    [Header("Stop attacking")]
    [SerializeField] public float disengageTimer;

    public bool rotateTowardUnit;
    [Header("Melee attack")]
    public float ColdownAfterMeleeAttack;
    public float meleeDamage;
    public float delayBeforeDamage = 0.4f;
    public bool isMeleeAttacking;
    public LayerMask meleeLayer;
    public float radiusOfMelee;
}
