using System;
using UnityEngine;
using UnityEngine.VFX;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] public bool bIsExplosive = false;
    [SerializeField] protected float ExplosionRadius = 20f;
    protected SphereCollider ExplosionColl;
    [SerializeField] protected VisualEffect ExplosionVFXref; 
    
    [SerializeField] protected float _maxLifetime = 5f;
    [SerializeField] public bool bHasGravity = true;
    [SerializeField] protected float _extraGravity = 9.81f;
    [SerializeField] public bool bCanBounce = false;
    [SerializeField] protected float _bounceMultiplier = 1.2f;
    public float ProjectileDamage = 15f;
    
    protected Rigidbody _rb;
    protected float _remaingLifetime;
    
    // Update is called once per frame
    protected void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _remaingLifetime = _maxLifetime;
    }
    
    protected void Update()
    {
        if (bHasGravity)
        {
            _rb.AddForce(Vector3.down * _extraGravity, ForceMode.Force);
        }
        
        _remaingLifetime -= Time.deltaTime;
        CheckLifetime();
    }

    protected void CheckLifetime()
    {
        if (_remaingLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (bIsExplosive)
        {
            Explode();
            return;
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (bIsExplosive)
        {
            Explode();
            return;
        }
        IDamageable damageable = other.GetComponent<Collider>().GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(this.gameObject, ProjectileDamage);
        }
        Destroy(gameObject);
    }

    protected void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider c in hits)
        {
            IDamageable damageable = c.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(gameObject, ProjectileDamage);
            }
        }
        ExplosionVFX();
        //TODO needs change
        Destroy(gameObject);
    }
    
    protected void ExplosionVFX()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 1);
        Gizmos.DrawSphere(transform.position, ExplosionRadius);
        if (ExplosionVFXref) ExplosionVFXref.Play();
    }
}
