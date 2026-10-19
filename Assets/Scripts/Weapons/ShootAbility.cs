using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : MonoBehaviour
{
    [SerializeField] public BaseWeapon EquippedGun;
    [SerializeField] public BaseWeapon[] OwnedGuns;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onLeftClickEvent += OnLeftClick;
        InputListener.onPress1Event += OnPress1;
        InputListener.onPress2Event += OnPress2;
        InputListener.onPress3Event += OnPress3;
        InputListener.onPress4Event += OnPress4;
        InputListener.onPressQEvent += OnPressQ;
        InputListener.onPressREvent += OnPressR;
        InputListener.onScrollUpEvent += OnScrollUp;
        InputListener.onScrollDownEvent += OnScrollDown;
        InputListener.onPressEscEvent += OnPressEsc;
        
        //InputListener.onLeftClickEvent += OnLeftClick;
        //InputListener.onLeftClickEvent += OnLeftClick;
    }

    protected void OnLeftClick(InputAction.CallbackContext context)
    {
        if (!EquippedGun) return;
        
        if (context.performed)
        {
            EquippedGun.StartFiring();
        }
        else if (context.canceled)
        {
            EquippedGun.StopFiring();
        }
    }
    
    protected void OnPress1(InputAction.CallbackContext context)
    {

    }

    protected void OnPress2(InputAction.CallbackContext context)
    {

    }

    protected void OnPress3(InputAction.CallbackContext context)
    {

    }

    protected void OnPress4(InputAction.CallbackContext context)
    {

    }

    protected void OnPressQ(InputAction.CallbackContext context)
    {

    }

    protected void OnPressR(InputAction.CallbackContext context)
    {

    }

    protected void OnScrollUp(InputAction.CallbackContext context)
    {

    }

    protected void OnScrollDown(InputAction.CallbackContext context)
    {

    }

    protected void OnPressEsc(InputAction.CallbackContext context)
    {

    }
}

