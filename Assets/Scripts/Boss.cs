using UnityEngine;

public class Boss : Unit
{
    public BaseWeapon weapon2;
    public GameObject obj;
    protected override AttackState CreateAttackState() => new BossAttackState(this, StateMachine, (BossAttackStateSO)attackStateSO);

    protected override void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Boss) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                textureLevel++;
                UpgradeTexture();
                break;
            case UpgradeCategory.Mesh:
                break;
            case UpgradeCategory.Sound:
                SoundManagerMino.Instance.SetBoss(0);
                break;
        }
    }
}
