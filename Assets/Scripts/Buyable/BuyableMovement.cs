using UnityEditor;
using UnityEngine;

public class BuyableMovement : MonoBehaviour, IBuyable
{
    [SerializeField] private MonoScript movementToAdd;

    public bool isBuyable = false;
    void Start()
    {
        
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

        System.Type type = movementToAdd.GetClass();
        if (type != null && typeof(BuyableBehavior).IsAssignableFrom(type))
        {
            BuyableBehavior weapon = GetComponent(type) as BuyableBehavior;

            if (weapon != null)
            {
                weapon.enabled = true;
            }
        }
        isBuyable = true;
        return true;
    }
}
