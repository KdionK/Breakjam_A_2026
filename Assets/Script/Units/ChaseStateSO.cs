using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Chase State")]
public class ChaseStateSO : StateScriptableObject
{
    public bool UseMeleeWeapon = false;
    public float DistanceMeleeToTurnAttack = 2;
}
