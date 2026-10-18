using UnityEngine;

public interface IBuyable
{
    public bool IsBuyable();
    public bool BuyThis(GameObject User);
}
