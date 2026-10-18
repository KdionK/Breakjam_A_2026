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
    [SerializeField] public GameObject target;
    public Launcher weapon;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        weapon = GetComponent<Launcher>();
        Agent = gameObject.GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine<State>();
        
        chaseState = new ChaseState(this, StateMachine, chaseStateSO);
        attackState = new AttackState(this, StateMachine, attackStateSO);
        
        StateMachine.Init(chaseState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
    }
    
    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicUpdate();
    }

    public DamageInfo TakeDamage(GameObject source, IDamageSource damageSource, float damage)
    {
        throw new System.NotImplementedException();
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
    
    private void OnDrawGizmos()
    {
        if(weapon == null)return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weapon.MaxAIRange);
    }
}
