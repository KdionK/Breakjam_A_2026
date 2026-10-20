using System;
using Unity.VisualScripting;
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
        Debug.Log("buyable movement" + _buyableMovement);
        Debug.Log("buyable generic" + _buyableGeneric);
        if (mapPopUp._component.Title == "A Dash" || mapPopUp._component.Title == "A Slide")
        {
           _buyableMovement = new BuyableMovement();
           this.gameObject.AddComponent<BuyableMovement>();
        }
        else
        {
            _buyableGeneric = new BuyableGeneric();
            _buyableGeneric.AddComponent<BuyableGeneric>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
