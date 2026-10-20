using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : MonoBehaviour
{
    [SerializeField] public BaseWeapon EquippedGun;
    [SerializeField] public BaseWeapon[] OwnedGuns;

    protected int CurrentEquippedIndex = 0;
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
    }

    protected void OnLeftClick(InputAction.CallbackContext context)
    {
        if (!EquippedGun) return;
        
        if (context.performed)
        {
            Debug.Log("Shoot1");
            EquippedGun.StartFiring();
        }
        else if (context.canceled)
        {
            EquippedGun.StopFiring();
        }
    }

    protected void EquipWeapon(int NewEquippedIndex)
    {
        if (OwnedGuns[NewEquippedIndex] == EquippedGun) return;
        
        //TODO play switch anim
        if (NewEquippedIndex > OwnedGuns.Length) NewEquippedIndex = 0;
        else if (NewEquippedIndex < 0) NewEquippedIndex = OwnedGuns.Length - 1;
        BaseWeapon newWeapon = OwnedGuns[NewEquippedIndex];
        EquippedGun.gameObject.transform.parent.gameObject.SetActive(false);
        EquippedGun = newWeapon;
        EquippedGun.gameObject.transform.parent.gameObject.SetActive(true);
    }
    
    protected void OnPress1(InputAction.CallbackContext context)
    {
        Debug.Log("Press1");
        CurrentEquippedIndex = 0;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPress2(InputAction.CallbackContext context)
    {
        Debug.Log("Press2");
        CurrentEquippedIndex = 1;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPress3(InputAction.CallbackContext context)
    {
        Debug.Log("Press3");
        CurrentEquippedIndex = 2;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPress4(InputAction.CallbackContext context)
    {

    }

    protected void OnPressQ(InputAction.CallbackContext context)
    {
        
    }

    protected void OnPressR(InputAction.CallbackContext context)
    {
        if (EquippedGun.bNeedsToReload && EquippedGun.CurrentAmmo < EquippedGun.MagSize) EquippedGun.StartReload();
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

