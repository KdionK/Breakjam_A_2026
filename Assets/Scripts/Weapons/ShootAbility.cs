using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootAbility : BuyableBehavior
{
    public bool bCanShoot = true;
    public bool bUnlockedGun0 = true;
    public bool bUnlockedGun1 = false;
    public bool bUnlockedGun2 = false;
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

        // if (EconomyManager.Instance)
        // {
        //     EconomyManager.Instance.CurrentAmmoText.text = EquippedGun.CurrentAmmo.ToString();
        //     EconomyManager.Instance.FullMagText.text = EquippedGun.MagSize.ToString();
        // }
        if (BattleStats.Instance) BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }
    
    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Player) return;

        switch (action.Category)
        {
            case UpgradeCategory.Unlock1:
                bCanShoot = true;
                bUnlockedGun0 = true;
                EquippedGun.gameObject.SetActive(true);
                break;
            case UpgradeCategory.Unlock2:
                bUnlockedGun1 = true;
                EquipWeapon(1);
                break;
            case UpgradeCategory.Unlock3:
                bUnlockedGun2 =  true;
                EquipWeapon(2);
                break;
        }
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
        if (!bCanShoot) return;
        if ((!bUnlockedGun0 && NewEquippedIndex == 0) ||
            (!bUnlockedGun1 && NewEquippedIndex == 1) ||
            (!bUnlockedGun2 && NewEquippedIndex == 2)) return;
        if (OwnedGuns[NewEquippedIndex] == EquippedGun) return;
        
        //TODO play switch anim
        if (NewEquippedIndex > OwnedGuns.Length) NewEquippedIndex = 0;
        else if (NewEquippedIndex < 0) NewEquippedIndex = OwnedGuns.Length - 1;
        BaseWeapon newWeapon = OwnedGuns[NewEquippedIndex];

        if (EconomyManager.Instance)
        {
            EconomyManager.Instance.FullMagText.text = newWeapon.CurrentAmmo.ToString();
            EconomyManager.Instance.FullMagText.text = newWeapon.MagSize.ToString();
        }
        
        EquippedGun.gameObject.SetActive(false);
        EquippedGun = newWeapon;
        EquippedGun.gameObject.SetActive(true);
    }
    
    protected void OnPress1(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun0) return;
        if (context.performed)
        {
            LastEquippedIndex = CurrentEquippedIndex;
            CurrentEquippedIndex = 0;
            EquipWeapon(CurrentEquippedIndex);
        }
    }

    protected void OnPress2(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun1) return;
        if (context.performed)
        {
            LastEquippedIndex = CurrentEquippedIndex;
            CurrentEquippedIndex = 1;
            EquipWeapon(CurrentEquippedIndex);
        }
    }

    protected void OnPress3(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun2) return;
        if (context.performed)
        {
            LastEquippedIndex = CurrentEquippedIndex;
            CurrentEquippedIndex = 2;
            EquipWeapon(CurrentEquippedIndex);
        }
    }

    protected void OnPress4(InputAction.CallbackContext context)
    {
    }

    protected void OnPressQ(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun0) return;
        if (context.performed)
        {
            EquipWeapon(LastEquippedIndex);
            (CurrentEquippedIndex, LastEquippedIndex) = (LastEquippedIndex, CurrentEquippedIndex);
        }
    }

    protected void OnPressR(InputAction.CallbackContext context)
    {
        if (!EquippedGun || !bUnlockedGun0) return;
        if (context.performed)
        {
            if (EquippedGun.bNeedsToReload && EquippedGun.CurrentAmmo < EquippedGun.MagSize) EquippedGun.StartReload();
        }
    }

    protected void OnScrollUp(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun2) return; //TODO better check
        if (context.performed)
        {
            LastEquippedIndex = CurrentEquippedIndex;
            CurrentEquippedIndex++;
            if (CurrentEquippedIndex >= OwnedGuns.Length) CurrentEquippedIndex = 0;
            EquipWeapon(CurrentEquippedIndex);
        }
    }

    protected void OnScrollDown(InputAction.CallbackContext context)
    {
        if (!bUnlockedGun2) return; //TODO better check
        if (context.performed)
        {
            LastEquippedIndex = CurrentEquippedIndex;
            CurrentEquippedIndex--;
            if (CurrentEquippedIndex < 0) CurrentEquippedIndex = OwnedGuns.Length - 1;
            EquipWeapon(CurrentEquippedIndex);
        }
    }

    protected void OnPressEsc(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Pause game");
        }
    }
}

