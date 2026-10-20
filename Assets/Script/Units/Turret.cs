using System;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    public GameObject turretRotation;
    public GameObject spawnLocation;
    public BaseWeapon weapon;
    private GameObject player;
    private bool isFiring = true;
    public float firingRadius = 10;
    private HealthComponent hpcomp;
    
    [Header("Visual")]
    private int textureLevel;
    public GameObject visualObject;
    [SerializeField] private GameObject[] visualUpgrades;

    private void Start()
    {
        player =  GameObject.FindWithTag("Player");
        hpcomp = GetComponent<HealthComponent>();
        hpcomp.OnDeath += OnDeath;
        
        textureLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Texture);
        
        UpgradeTexture();
        
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, spawnLocation.transform.position) < firingRadius)
        {
            Vector3 directionToTarget =
                player.transform.position - transform.position;

            directionToTarget.y = 0f;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                turretRotation.transform.rotation = Quaternion.Slerp(
                    turretRotation.transform.rotation,
                    Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f),
                    Time.deltaTime * 5f
                );
            }

            if (!isFiring)
            {
                weapon.StartFiring();
                isFiring = true;
            }
        }
        else
        {
            weapon.StopFiring();
            isFiring = false;
        }
    }

    public DamageInfo TakeDamage(GameObject source, float damage)
    {
        Debug.Log("Turret");
        DamageInfo damageInfo = new DamageInfo()
        {
            finalDamage = damage,
            Source = source,
            Target = this.gameObject,
        };
        hpcomp.ModifyHealth((int)damage);
        return damageInfo;
    }

    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if(action.Type != UnitType.CommonEnemy) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                textureLevel++;
                UpgradeTexture();
                break;
            case UpgradeCategory.Mesh:
                break;
            case UpgradeCategory.Sound:
                break;
        }
    }
    
    private void UpgradeTexture()
    {
        
        if (textureLevel <= 0 || visualUpgrades.Length == 0) return;
        Debug.Log("Upgrade Texture");
        int i = Mathf.Min(textureLevel, visualUpgrades.Length) - 1;
        visualObject.SetActive(false);
        visualUpgrades[i].SetActive(true);
        visualObject = visualUpgrades[i];
    }
    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        throw new System.NotImplementedException();
    }

    public HealthComponent GetHealth()
    {
        throw new System.NotImplementedException();
    }

    private void OnDeath()
    {
        BattleStats.Instance.Unsubscribe<BuyAction>(OnBoughtEvent);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRadius);
    }
}
