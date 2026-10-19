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

    private Vector2 ReadWASD()
    {
        var kb = Keyboard.current;
        if (kb == null) return Vector2.zero;

        float x = 0f, y = 0f;
        if (kb.wKey.isPressed) y += 1f;
        if (kb.sKey.isPressed) y -= 1f;
        if (kb.dKey.isPressed) x += 1f;
        if (kb.aKey.isPressed) x -= 1f;

        return new Vector2(x, y);
    }

    // private void Dashing()
    // {
    //     rb.linearVelocity = new Vector3(_dashDirection.x * _dashForce, 0f, _dashDirection.z * _dashForce);
    // }
    private void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // ignore started / canceled (release)
        
        if (!_dashBuy || !_canDash) return;
        if (_moveScript._sliding) return;

        Vector3 dir = GetDashDirection();
        if (dir.sqrMagnitude < 0.01f)
        {
            dir = transform.forward;
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

            rb.linearVelocity = new Vector3(_dashDirection.x * _dashForce, 0f, _dashDirection.z * _dashForce);
            _dashTimer -= Time.fixedDeltaTime;
            if (_dashTimer <= 0f) _moveScript._isDashing = false;
        }

        if (!_canDash)
        {
            _dashTime -= Time.fixedDeltaTime;
            if (_dashTime <= 0f) _canDash = true;
        }
    }

    private Vector3 GetDashDirection()
    {
        Vector2 input = ReadWASD();

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * input.y + right * input.x;
    }
}
