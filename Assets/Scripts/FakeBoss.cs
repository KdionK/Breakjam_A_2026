using UnityEngine;

public class FakeBoss : MonoBehaviour
{
     public StateMachine<State> StateMachine;
    [HideInInspector]
   
    void Start()
    {
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    

    

   
    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Boss) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                break;
            case UpgradeCategory.Mesh:
                break;
            case UpgradeCategory.Sound:
                SoundManagerMino.Instance.SetBoss(0);
                break;
        }
    }
}
