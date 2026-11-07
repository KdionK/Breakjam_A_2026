using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuyableMovement : MonoBehaviour, IBuyable
{
    [SerializeField] public MonoScript movementToAdd;
    [SerializeField] public BuyableBehavior buyableMovement;
    [SerializeField] private int _price;


    public bool isBuyable = false;
    MapPopUp mapPopUp;
    public bool _isDynamic = true;
    void Start()
    {
        mapPopUp = GetComponentInParent<MapPopUp>();
        if (_isDynamic)
        {
            _price = mapPopUp._component.Price;
            GameObject player = GameObject.FindWithTag("Player");

            if (mapPopUp._component.Title == "A Dash")
                buyableMovement = player.GetComponent<Dash>();
            else if (mapPopUp._component.Title == "A Slide")
                buyableMovement = player.GetComponent<Slide>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsBuyable()
    {
        return !isBuyable;
    }

    public bool BuyThis(GameObject User)
    {
        if (movementToAdd == null)
            return  false;
        if (EconomyManager.Instance.Currency >= _price)
        {
            EconomyManager.Instance.ChangeCurrency(-_price);
            buyableMovement.enabled = true;
            isBuyable = true;
            return true;
            // System.Type type = movementToAdd.GetClass();
            // Debug.Log("adding movement");
            // if (type != null && typeof(BuyableBehavior).IsAssignableFrom(type))
            // {
            //     BuyableBehavior movement = User.GetComponent(type) as BuyableBehavior;
            //
            //     if (movement != null)
            //     {
            //         movement.enabled = true;
            //     }
            // }
        }
        return false;
        bool IsBuyable() => !isBuyable;
        // isBuyable = true;
        // return true;
    }
}
