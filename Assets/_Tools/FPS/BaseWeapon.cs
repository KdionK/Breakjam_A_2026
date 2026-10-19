using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] [Tooltip("If something else than the player is using this weapon, this must be True")]
    bool bIsAIControlled = false;
    [SerializeField] [Tooltip("The range that the AI can start shooting the player")]
    public float MaxAIRange = 400f;

    [SerializeField] [Tooltip("The prefab of the projectile")]
    GameObject _projectile;

    [SerializeField] [Tooltip("The position and rotation where the projectile will be spawned")]
    Transform _projectileSpawn;

    [SerializeField] [Tooltip("The force of the launch")]
    float _launchForce = 100f;

    
    [SerializeField] float _damage = 15f;

    //Ammo
    [SerializeField] [Tooltip("The max ammo numb before reloading")]
    int _magSize = 20;
    public int _currentAmmo;

    //Reload
    [SerializeField] [Tooltip("False if continuous fire")]
    bool bNeedsToReload = true;
    [SerializeField] [Tooltip("The reload time in seconds")]
    float _reloadTime = 2f;
    public float _currentReloadTimer = 0f;
    private bool bReloading = false;

    //Fire rate
    [SerializeField] [Tooltip("Automatic weapon or single fire? Single fire is also capped at" +
                              " Max Fire Rate. AIs ignore this, they are always auto")]
    bool bIsAuto = true;
    [SerializeField] [Tooltip("Max number of shots per second")]
    float _maxFireRate = 5f;
    [SerializeField] [Tooltip("Max number of shots per second when weapon is held by AI")]
    float _maxAIFireRate = 2f;
    private float _intervalBetweenShots;
    private float _nextFireTime = 0f;

    private bool bTryingToFire = false;
    
    
    private void Start()
    {
        //move to char

        _currentAmmo = _magSize;
        if (!bIsAIControlled)
        {
            _intervalBetweenShots = 1 / _maxFireRate;
        }
        else
        {
            _intervalBetweenShots = 1 / _maxAIFireRate;
        }
    }

    protected void Update()
    {
        TryFire();
    }

    protected void TryFire()
    {
        if (bReloading)
        {
            if (_currentReloadTimer >= _reloadTime)
            {
                EndReload();
            }
            else
            {
                _currentReloadTimer += Time.deltaTime;
                return;
            }
        }

        if (!bTryingToFire) return;
        if (Time.time < _nextFireTime) return;

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
        _currentAmmo = _magSize;
        _currentReloadTimer = 0;
        bReloading = false;
    }

    protected void Fire()
    {
        SpawnProjectile();

        _nextFireTime = Time.time + _intervalBetweenShots;

        if (bNeedsToReload)
        {
            _currentAmmo--;
            //Debug.Log("Ammo: " + _currentAmmo + "/" + _magSize);
            if (_currentAmmo <= 0)
            {
                StartReload();
            }
        }
    }

    protected void SpawnProjectile()
    {
        GameObject p = Instantiate(_projectile, _projectileSpawn.position, _projectileSpawn.rotation);
        BaseProjectile pScript = p.GetComponent<BaseProjectile>();
        pScript.ProjectileDamage = _damage;
        p.GetComponent<Rigidbody>().AddForce(_projectileSpawn.forward * _launchForce, ForceMode.VelocityChange);
    }
    

    public void StartFiring() //call this to auto fire if AI, or single fire if player and bIsAuto = false
    {
        bTryingToFire = true;
    }

    public void StopFiring()
    {
        bTryingToFire = false;
        _nextFireTime = 0f;
    }
}