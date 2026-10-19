using UnityEngine;

[CreateAssetMenu(menuName = "States/Boss Attack State")]
public class BossAttackStateSO : AttackStateSO
{
    [Header("Boss")]
    public float rangedDuration = 5f;
    public float meleeStoppingDistance = 2f; 
}