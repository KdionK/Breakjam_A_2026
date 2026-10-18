using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] Move _moveScript;
    private Rigidbody rb;
    [SerializeField] float _dashForce = 20f;
    [SerializeField] float _dashDuration = 0.15f;
    [SerializeField] float _dashCooldown = 4f;
    [SerializeField] bool _dashBuy = true;
    private float _dashTime = 0f;
    private bool _canDash = true;
    private float _dashTimer;
    private Vector3 _dashDirection;
    private Vector2 _moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onDashEvent += OnDash;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleDash();
    }

    private void Dashing()
    {
        rb.linearVelocity = new Vector3(_dashDirection.x * _dashForce, 0f, _dashDirection.z * _dashForce);
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        if (!_dashBuy || !_canDash) return;

        Vector3 dir = GetMoveDirection();
        if (dir.sqrMagnitude < 0.01f)
        {
            dir = _cam.forward;
            // dir.y = 0f;
        }
        _dashDirection = dir.normalized;

        _moveScript._isDashing = true;
        _dashTimer = _dashDuration;
        _dashTime = _dashCooldown;
        _canDash = false;

    }

    private void HandleDash()
    {
        if (_moveScript._isDashing)
        {

            rb.linearVelocity = new Vector3(_dashDirection.x * _dashForce,0f,_dashDirection.z * _dashForce);
            _dashTimer -= Time.fixedDeltaTime;
            if (_dashTimer <= 0f) _moveScript._isDashing = false;
        }

        if (!_canDash)
        {
            _dashTime -= Time.fixedDeltaTime;
            if (_dashTime <= 0f) _canDash = true;
        }
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 forward = _cam.forward;
        Vector3 right = _cam.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * _moveDirection.y + right * _moveDirection.x;
    }
}
