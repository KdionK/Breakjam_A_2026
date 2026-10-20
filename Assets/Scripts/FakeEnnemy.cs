using UnityEngine;

public class FakeEnnemy : MonoBehaviour
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
        if (action.Type != UnitType.CommonEnemy) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                break;
            case UpgradeCategory.Mesh:
                break;
            case UpgradeCategory.Sound:
                SoundManagerMino.Instance.SetEnnemi(0);
                break;
        }
    }
}
