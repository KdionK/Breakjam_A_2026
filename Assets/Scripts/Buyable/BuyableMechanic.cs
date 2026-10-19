using System;
using UnityEditor;
using UnityEngine;

public class BuyableMechanic : MonoBehaviour, IBuyable
{
    [SerializeField] private MonoScript weaponToAdd;

    public bool isBuyable = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        if (weaponToAdd == null)
            return  false;

        System.Type type = weaponToAdd.GetClass();
        if (type != null && typeof(BaseWeapon).IsAssignableFrom(type))
        {
            BaseWeapon weapon = GetComponent(type) as BaseWeapon;

            if (weapon != null)
            {
                weapon.enabled = true;
            }
        }
        isBuyable = true;
        return true;
    }
}
