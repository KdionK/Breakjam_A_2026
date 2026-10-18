using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Attack State")]
public class AttackStateSO : StateScriptableObject
{
    [Header("Stop attacking")]
    [SerializeField] private float disengageTimer;
}
