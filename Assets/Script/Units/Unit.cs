using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IDamageable
{
    public StateMachine<State> StateMachine;
    [HideInInspector]
    public NavMeshAgent Agent;
    
    public ChaseState chaseState;
    public AttackState attackState;
    [Header("State Configs")]
    [SerializeField] private ChaseStateSO chaseStateSO;
    [SerializeField] private AttackStateSO attackStateSO;
    
    [Header("targetting")]
    [HideInInspector] public GameObject target;
    [HideInInspector]
    public BaseWeapon weapon;

    public GameObject spawnLocation;
    public GameObject visualObject;

    private HealthComponent healthComponent;
    [Header("texture")]
    private int textureLevel;
    [SerializeField] private GameObject[] visualUpgrades;
    void Start()
    {
        healthComponent = GetComponent<HealthComponent>();
        target = GameObject.FindGameObjectWithTag("Player");
        weapon = GetComponent<BaseWeapon>();
        Agent = gameObject.GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine<State>();
        
        chaseState = new ChaseState(this, StateMachine, chaseStateSO);
        attackState = new AttackState(this, StateMachine, attackStateSO);
        
        StateMachine.Init(chaseState);
        
        textureLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Texture);
        
        UpgradeTexture();
        
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)return;
        StateMachine.CurrentState.FrameUpdate();
    }
    
    public virtual void FixedUpdate()
    {
        if(target == null)return;
        StateMachine.CurrentState.PhysicUpdate();
    }

    public DamageInfo TakeDamage(GameObject source, float damage)
    {
        DamageInfo damageInfo = new DamageInfo()
        {
            finalDamage = damage,
            Source = source,
            Target = this.gameObject,
        };
        healthComponent.ModifyHealth((int)damage);
        return damageInfo;
    }

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        throw new System.NotImplementedException();
    }

    public bool IsWounded()
    {
        throw new System.NotImplementedException();
    }

    public HealthComponent GetHealth()
    {
        throw new System.NotImplementedException();
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
        int i = Mathf.Min(textureLevel, visualUpgrades.Length) - 1;
        visualObject.SetActive(false);
        visualUpgrades[i].SetActive(true);
        visualObject = visualUpgrades[i];
    }
    
    private void OnDrawGizmos()
    {
        if(weapon == null)return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weapon.MaxAIRange);
    }
}
