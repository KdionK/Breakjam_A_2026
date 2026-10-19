using UnityEditor;
using UnityEngine;

public class BuyableMovement : MonoBehaviour, IBuyable
{
    [SerializeField] public MonoScript movementToAdd;

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
        Debug.Log("adding movement");
        if (type != null && typeof(BuyableBehavior).IsAssignableFrom(type))
        {
            BuyableBehavior movement = User.GetComponent(type) as BuyableBehavior;

            if (movement != null)
            {
                movement.enabled = true;
            }
        }
        isBuyable = true;
        return true;
    }
}
