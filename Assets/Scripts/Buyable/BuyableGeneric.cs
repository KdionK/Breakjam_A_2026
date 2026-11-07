using UnityEngine;

public class BuyableGeneric : MonoBehaviour, IBuyable
{
    public UnitType type;
    public UpgradeCategory category;
    [SerializeField] private int _price = 10;
    MapPopUp mapPopUp;
    public bool _isDynamic = true;

    private bool _alreadyBought = false;
    void Start()
    {
        mapPopUp = GetComponentInParent<MapPopUp>();
        if (_isDynamic)
        {
            type = mapPopUp._component.UnitType;
            category = mapPopUp._component.CategoryType;
            _price = mapPopUp._component.Price;
        }
        // !_isDynamic is for static upgrades like door opens;
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
        if (EconomyManager.Instance.Currency >= _price)
        {
            EconomyManager.Instance.ChangeCurrency(-_price);
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

        return false;
    }
}
