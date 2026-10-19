using UnityEngine;

public class JumpEnabler : BuyableBehavior
{
  
    private Move _move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _move = GetComponent<Move>();
        _move._jumpBuy = true;
    }
}
