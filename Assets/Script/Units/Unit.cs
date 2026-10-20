using Unity.VisualScripting;
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
    [SerializeField] protected ChaseStateSO chaseStateSO;
    [SerializeField] protected AttackStateSO attackStateSO;

    [Header("targetting")]
    [HideInInspector] public GameObject target;
    public BaseWeapon weapon;

    public GameObject spawnLocation;


    private HealthComponent healthComponent;
    [Header("Visual")]
    protected int textureLevel;
    public GameObject visualObject;
    [SerializeField] private GameObject[] visualUpgrades;
    public Animator anim;

    [Header("money")]
    public int moneyOnDeath;


    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip droneShoot;
    [SerializeField] private AudioClip enemySpawn;
    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip enemyPunch;

    public AudioSource AudioSource => _audioSource;
    public AudioClip DroneShoot => droneShoot;
    public AudioClip EnemySpawn => enemySpawn;
    public AudioClip EnemyDeath => enemyDeath;
    public AudioClip EnemyPunch => enemyPunch;


    void Start()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += Death;
        target = GameObject.FindGameObjectWithTag("Player");
        //weapon = GetComponent<BaseWeapon>();
        Agent = gameObject.GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine<State>();

        chaseState = CreateChaseState();
        attackState = CreateAttackState();

        StateMachine.Init(chaseState);

        textureLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Texture);

        UpgradeTexture();

        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    protected virtual ChaseState CreateChaseState() => new ChaseState(this, StateMachine, chaseStateSO);
    protected virtual AttackState CreateAttackState() => new AttackState(this, StateMachine, attackStateSO);

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        StateMachine.CurrentState.FrameUpdate();
    }

    public virtual void FixedUpdate()
    {
        if (target == null) return;
        StateMachine.CurrentState.PhysicUpdate();
    }

    public DamageInfo TakeDamage(GameObject source, float damage)
    {
        Debug.Log("unit take damage");
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
    protected virtual void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.CommonEnemy) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                textureLevel++;
                UpgradeTexture();
                break;
            case UpgradeCategory.Mesh:
                break;
            case UpgradeCategory.Sound:
                SoundManagerMino.Instance.SetEnnemi(0);
                break;
        }
    }
    protected void UpgradeTexture()
    {
        if (textureLevel <= 0 || visualUpgrades.Length == 0) return;
        int i = Mathf.Min(textureLevel, visualUpgrades.Length) - 1;
        visualObject.SetActive(false);
        visualUpgrades[i].SetActive(true);
        visualObject = visualUpgrades[i];
    }

    public void Death()
    {
        EconomyManager.Instance.ChangeCurrency(moneyOnDeath);
        BattleStats.Instance.Unsubscribe<BuyAction>(OnBoughtEvent);
        SoundManagerMino.Instance.PlaySound(enemyDeath, _audioSource);
        Destroy(this.gameObject);
    }
    private void OnDrawGizmos()
    {
        if (weapon == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weapon.MaxAIRange);
    }
}
