using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] float _extraGravity = 9.81f;
    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * _extraGravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 bounce = Vector3.Reflect(GetComponent<Rigidbody>().linearVelocity, collision.contacts[0].normal);
        GetComponent<Rigidbody>().linearVelocity = bounce * 1.2f;
    }
}
