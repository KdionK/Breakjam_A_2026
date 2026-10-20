using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class BaseWeapon : BuyableBehavior
{
    [Header("AI")]
    [SerializeField] [Tooltip("If something else than the player is using this weapon, this must be True")]
    bool bIsAIControlled = false;
    [SerializeField] [Tooltip("The range that the AI can start shooting the player")]
    public float MaxAIRange = 400f;
    
    [Header("Shoot Mode")]
    [SerializeField] [Tooltip("If true, will be hitscan instead of shooting a projectile")]
    bool bIsHitscan = false;
    [Header("Projectile")]
    [SerializeField] [Tooltip("The prefab of the projectile")]
    GameObject Projectile;
    [SerializeField] [Tooltip("The position and rotation where the projectile will be spawned. For a hitscan weapon in a player, put the camera transform")]
    Transform ProjectileSpawnTransform;
    [SerializeField] [Tooltip("The force of the launch")]
    float LaunchForce = 100f;
    
    [Header("Visual Effects")]
    [SerializeField] protected VisualEffect MuzzleFlash;


    [Header("Damage")]
    [FormerlySerializedAs("_damage")] [SerializeField] [Tooltip("Damage when hit target is closer than MaxDamageRange. Will also override projectile damage if projectile weapon")]
    float Damage = 15f;
    [Header("Damage Hitscan")]
    [SerializeField] [Tooltip("Above this range, damage starts decreasing until it reaches MinDamage")]
    float RangeOfMaxDamage = 10f;
    [SerializeField] [Tooltip("Above this range, no damage.")]
    float DamageMaxRange = 100f;
    [SerializeField] [Tooltip("Damage when hit target is just before the max range")]
    float MinDamage = 1f;
    
    [Header("Ammo and reload")]
    [SerializeField] [Tooltip("False if continuous fire")]
    public bool bNeedsToReload = true;
    [SerializeField] [Tooltip("The max ammo numb before reloading")]
    public int MagSize = 20;
    public int CurrentAmmo;
    [SerializeField] [Tooltip("The reload time in seconds")]
    float ReloadTime = 2f;
    public float CurrentReloadTimer = 0f;
    private bool bReloading = false;

    [Header("Fire Rate")]
    [Tooltip("Automatic weapon or single fire? Single fire is also capped at" +
             " Max Fire Rate. AIs ignore this, they are always auto")]
    bool bIsAuto = true;
    [SerializeField] [Tooltip("Max number of shots per second")]
    float MaxFireRate = 5f;
    [SerializeField] [Tooltip("Max number of shots per second when weapon is held by AI")]
    float MaxAIFireRate = 2f;
    private float IntervalBetweenShots;
    private float NextFireTime = 0f;

    private bool bTryingToFire = false;
    
    [Header("Upgrades")]
    [SerializeField] private GameObject[] visualUpgrades; // added by mino
    protected int textureLevel = 0;
    public GameObject visualObject;
    [FormerlySerializedAs("bUnlockedMuzzleFlash")] public bool bUnlockedVFX = false;
    
    private void Start()
    {
        CurrentAmmo = MagSize;
        if (!bIsAIControlled)
        {
            IntervalBetweenShots = 1 / MaxFireRate;
        }
        else
        {
            IntervalBetweenShots = 1 / MaxAIFireRate;
        }

        if (BuyHistory.Instance)
        {
            textureLevel = BuyHistory.Instance.GetLevel(UnitType.Player, UpgradeCategory.Texture);
            UpgradeTexture();
        }
        if (BattleStats.Instance) BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    protected void Update()
    {
        TryFire();
    }

    protected void TryFire()
    {
        if (bReloading)
        {
            if (CurrentReloadTimer >= ReloadTime)
            {
                EndReload();
            }
            else
            {
                CurrentReloadTimer += Time.deltaTime;
                return;
            }
        }

        if (!bTryingToFire) return;
        if (Time.time < NextFireTime) return;

        if (!bIsAIControlled)
        {
            if (bIsAuto)
            {
                Fire();
            }
            else
            {
                Fire();
                bTryingToFire = false;
            }
        }
        else // if AI
        {
            Fire();
        }
    }

    public void StartReload()
    {
        bReloading = true;
    }

    public void EndReload()
    {
        CurrentAmmo = MagSize;
        CurrentReloadTimer = 0;
        if (EconomyManager.Instance) EconomyManager.Instance.FullMagText.text = CurrentAmmo.ToString();
        bReloading = false;
    }

    protected void Fire()
    {
        if (bIsHitscan)
        {
            //Debug.Log("ShootRaycast");
            SpawnRaycast();
        }
        else
        {
            //Debug.Log("ShootProjectile");
            SpawnProjectile();
        }
        
        NextFireTime = Time.time + IntervalBetweenShots;

        if (bUnlockedVFX)
        {
            VFXMuzzleFlash();
        }
        
        if (bNeedsToReload)
        {
            CurrentAmmo--;
            if (EconomyManager.Instance) EconomyManager.Instance.CurrentAmmoText.text = CurrentAmmo.ToString();
            //Debug.Log("Ammo: " + _currentAmmo + "/" + _magSize);
            if (CurrentAmmo <= 0)
            {
                StartReload();
            }
        }
    }

    protected void SpawnProjectile()
    {
        GameObject p = Instantiate(Projectile, ProjectileSpawnTransform.position, ProjectileSpawnTransform.rotation);
        BaseProjectile pScript = p.GetComponent<BaseProjectile>();
        pScript.ProjectileDamage = Damage;
        p.GetComponent<Rigidbody>().AddForce(ProjectileSpawnTransform.forward * LaunchForce, ForceMode.VelocityChange);
    }

    protected void SpawnRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(ProjectileSpawnTransform.position, ProjectileSpawnTransform.forward, out hit, DamageMaxRange))
        {
            if (!hit.collider) return;
            
            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float t = Mathf.InverseLerp(RangeOfMaxDamage, DamageMaxRange, hit.distance);
                float finalDamage = Mathf.Lerp(Damage, MinDamage, t);
                // float v = Mathf.InverseLerp(DamageMaxRange, MaxDamageMaxRange, hit.distance);
                // float finalDamage = v * MaxDamage;
                damageable.TakeDamage(gameObject, finalDamage);
                
                Debug.Log("Dist: " + hit.distance + ", finalDamage: " + finalDamage);
                Debug.DrawLine(ProjectileSpawnTransform.position, hit.point, Color.red, 2f, false);

            }
            else
            {
                Debug.Log("Dist: " + hit.distance + "no damage");
                Debug.DrawLine(ProjectileSpawnTransform.position, hit.point, Color.yellow, 2f, false);
            }
        }
        else
        {
            Debug.Log("no hit");
            Debug.DrawLine(ProjectileSpawnTransform.position,ProjectileSpawnTransform.position + ProjectileSpawnTransform.forward * DamageMaxRange, Color.white, 2f, false);
        }

    }

    protected void VFXMuzzleFlash()
    {
        MuzzleFlash.Reinit();
        MuzzleFlash.Play();
    }
    
    public void StartFiring()
    {
        bTryingToFire = true;
    }

    public void StopFiring()
    {
        bTryingToFire = false;
        NextFireTime = 0f;
    }
    
    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Player) return;
        if (bIsAIControlled) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                textureLevel++;
                UpgradeTexture();
                break;
            case UpgradeCategory.Vfx:
                ActivateWeaponVFX();
                break;
        }
    }

    protected void ActivateWeaponVFX()
    {
        bUnlockedVFX = true;
    }
    protected void UpgradeTexture()
    {
        if (textureLevel <= 0 || visualUpgrades.Length == 0) return;
        int i = Mathf.Min(textureLevel, visualUpgrades.Length) - 1;
        visualObject.SetActive(false);
        visualUpgrades[i].SetActive(true);
        visualObject = visualUpgrades[i];
    }
}