using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : MonoBehaviour
{
    [SerializeField] public BaseWeapon EquippedGunRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onLeftClickEvent += OnLeftClick;
    }

    protected void OnLeftClick(InputAction.CallbackContext context)
    {
        if (!EquippedGunRef) return;
        
        if (context.performed)
        {
            EquippedGunRef.StartFiring();
        }
        else if (context.canceled)
        {
            EquippedGunRef.StopFiring();
        }
    }
}

