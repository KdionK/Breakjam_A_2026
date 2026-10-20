using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : MonoBehaviour
{
    public bool bCanShoot = true;
    public bool bUnlockedGun0 = true;
    public bool bUnlockedGun1 = true;
    public bool bUnlockedGun2 = true;
    [SerializeField] public BaseWeapon EquippedGun;
    [SerializeField] public BaseWeapon[] OwnedGuns;

    protected int CurrentEquippedIndex = 0;
    protected int LastEquippedIndex = 0;
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
        if (!EquippedGun || !bCanShoot) return;
        
        if (context.performed)
        {
            //Debug.Log("Shoot1");
            EquippedGun.StartFiring();
        }
        else if (context.canceled)
        {
            EquippedGun.StopFiring();
        }
    }

    protected void EquipWeapon(int NewEquippedIndex)
    {
        if ((!bUnlockedGun0 && NewEquippedIndex == 0) ||
            (!bUnlockedGun1 && NewEquippedIndex == 1) ||
            (!bUnlockedGun2 && NewEquippedIndex == 2)) return;
        if (OwnedGuns[NewEquippedIndex] == EquippedGun) return;
        
        //TODO play switch anim
        if (NewEquippedIndex > OwnedGuns.Length) NewEquippedIndex = 0;
        else if (NewEquippedIndex < 0) NewEquippedIndex = OwnedGuns.Length - 1;
        BaseWeapon newWeapon = OwnedGuns[NewEquippedIndex];
        EquippedGun.gameObject.SetActive(false);
        EquippedGun = newWeapon;
        EquippedGun.gameObject.SetActive(true);
    }
    
    protected void OnPress1(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun0) return;
        LastEquippedIndex = CurrentEquippedIndex;
        CurrentEquippedIndex = 0;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPress2(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun1) return;
        LastEquippedIndex = CurrentEquippedIndex;
        CurrentEquippedIndex = 1;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPress3(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun2) return;
        LastEquippedIndex = CurrentEquippedIndex;
        CurrentEquippedIndex = 2;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPress4(InputAction.CallbackContext context)
    {
    }

    protected void OnPressQ(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun0) return;
        EquipWeapon(LastEquippedIndex);
        (CurrentEquippedIndex, LastEquippedIndex) = (LastEquippedIndex, CurrentEquippedIndex);
    }

    protected void OnPressR(InputAction.CallbackContext context)
    {
        if (!EquippedGun || !bUnlockedGun0) return;
        if (EquippedGun.bNeedsToReload && EquippedGun.CurrentAmmo < EquippedGun.MagSize) EquippedGun.StartReload();
    }

    protected void OnScrollUp(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun2) return; //TODO better check
        LastEquippedIndex = CurrentEquippedIndex;
        CurrentEquippedIndex++;
        if (CurrentEquippedIndex >= OwnedGuns.Length) CurrentEquippedIndex = 0;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnScrollDown(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun2) return; //TODO better check
        LastEquippedIndex = CurrentEquippedIndex;
        CurrentEquippedIndex--;
        if (CurrentEquippedIndex < 0) CurrentEquippedIndex = OwnedGuns.Length - 1;
        EquipWeapon(CurrentEquippedIndex);
    }

    protected void OnPressEsc(InputAction.CallbackContext context)
    {
        Debug.Log("Pause game");
    }
}

