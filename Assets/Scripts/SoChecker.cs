using UnityEngine;

public class SoChecker : MonoBehaviour
{
    BuyableMovement _buyableMovement;
    BuyableGeneric _buyableGeneric;
    MapPopUp mapPopUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapPopUp = GetComponentInParent<MapPopUp>();
        _buyableMovement = GetComponent<BuyableMovement>();
        _buyableGeneric = GetComponent<BuyableGeneric>();
        if (mapPopUp._component.Title == "A Dash" || mapPopUp._component.Title == "A Slide")
        {
            _buyableGeneric.enabled = false;
            _buyableMovement.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
