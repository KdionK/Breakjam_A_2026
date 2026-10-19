using UnityEngine;

public class TestEnemyZach : MonoBehaviour, IDamageable
{
    [SerializeField] private Material[] mats;
    private int textureLevel;
    private int meshLevel;
    private int soundLevel;
    private Renderer rend;
    public HealthComponent hpcomponent;
    private HealthComponent healthComponent;
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += Death;
        
        textureLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Texture);
        meshLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Mesh);
        soundLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Sound);

        UpgradeTexture();
        UpgradeMesh();
        UpgradeSound();
        
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
        
        hpcomponent = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                meshLevel++;
                UpgradeMesh();
                break;
            case UpgradeCategory.Sound:
                soundLevel++;
                UpgradeSound();
                break;
        }
    }

    private void UpgradeTexture()
    {
        if (textureLevel <= 0 || mats.Length == 0) return;
        int i = Mathf.Min(textureLevel, mats.Length) - 1;
        rend.material = mats[i];
    }
    private void UpgradeMesh()
    {
        
    }
    private void UpgradeSound()
    {
        
    }

    public DamageInfo TakeDamage(GameObject source, float damage)
    {
        DamageInfo dinfo = new DamageInfo();
        hpcomponent.ModifyHealth((int)damage);
        return dinfo;
    }

    public void Death()
    {
        BattleStats.Instance.Unsubscribe<BuyAction>(OnBoughtEvent);
        Destroy(this.gameObject);
    }
    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        return null;
    }

    public HealthComponent GetHealth()
    {
        return hpcomponent;
    }
}
