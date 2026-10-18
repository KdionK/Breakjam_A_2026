using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    [SerializeField][Tooltip("The prefab of the projectile")] GameObject _projectile;
    [SerializeField][Tooltip("The position where the projectile will be spawned")] Transform _projectileSpawn;
    [SerializeField][Tooltip("The force of the launch")] float _launchForce = 1000f;

    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Launch();
        }
    }

    void Launch()
    {
        GameObject p = Instantiate(_projectile, _projectileSpawn.position, _projectileSpawn.rotation);
        p.GetComponent<Rigidbody>().AddForce(_projectileSpawn.forward * _launchForce, ForceMode.VelocityChange);
    }
}
