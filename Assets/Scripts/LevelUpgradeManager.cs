using UnityEngine;

public class LevelUpgradeManager : BuyableBehavior
{
    [SerializeField] private GameObject DefaultLevelTextures;
    [SerializeField] private GameObject UpgradedLevelTextures;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BattleStats.Instance) BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Npc) return;

        switch (action.Category)
        {
            case UpgradeCategory.Level:
                UpgradeLevelTextures();
                return;
        }
    }

    private void UpgradeLevelTextures()
    {
        DefaultLevelTextures.SetActive(false);
        UpgradedLevelTextures.SetActive(true);
    }
    
    
}
