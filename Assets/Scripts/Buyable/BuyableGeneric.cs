using UnityEngine;

public class BuyableGeneric : MonoBehaviour, IBuyable
{
    public UnitType type;
    public UpgradeCategory category;

    private bool _alreadyBought = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsBuyable()
    {
        return !_alreadyBought;
    }

    public bool BuyThis(GameObject User)
    {
        BuyAction newBuy = new BuyAction
        {
            Actor = User,
            BoughtItem = this,
            Type = type,
            Category = category,
        };
        
        BattleStats.Instance.Broadcast(newBuy);
        return true;
    }
}
