using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float _maxLifetime = 5f;
    [SerializeField] public bool bHasGravity = true;
    [SerializeField] protected float _extraGravity = 9.81f;
    [SerializeField] public bool bCanBounce = true;
    [SerializeField] protected float _bounceMultiplier = 1.2f;
    public float ProjectileDamage = 15f;
    
    protected Rigidbody _rb;
    protected float _remaingLifetime;
    
    // Update is called once per frame
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _remaingLifetime = _maxLifetime;
    }
    
    void Update()
    {
        if (bHasGravity)
        {
            _rb.AddForce(Vector3.down * _extraGravity, ForceMode.Force);
        }
        
        _remaingLifetime -= Time.deltaTime;
        CheckLifetime();
    }

    void CheckLifetime()
    {
        if (_remaingLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Do damage here
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(this.gameObject, ProjectileDamage);
        }
        if (bCanBounce)
        {
            Vector3 bounce = Vector3.Reflect(GetComponent<Rigidbody>().linearVelocity, collision.contacts[0].normal);
            GetComponent<Rigidbody>().linearVelocity = bounce * _bounceMultiplier;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
