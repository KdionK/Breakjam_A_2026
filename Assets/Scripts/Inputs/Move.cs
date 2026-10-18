
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] private LayerMask _groundLayer;
    [Header("buyChecks")]
    [SerializeField] bool _jumpBuy = true;
    [SerializeField] bool _dashBuy = true;

    [Header("base movement")]
    [SerializeField] float _speed = 5;
    [Header("Jump")]
    [SerializeField] float _jumpForce = 3;
    [SerializeField] private int _maxAirJumps = 0;
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private float _jumpBufferTime = 0.1f;    // saut mémorisé si pressé juste avant d'atterrir
    [SerializeField] float _rayDownDistance = 5f;
    [Header("Dash")]
    [SerializeField] float _dashForce = 20f;
    [SerializeField] float _dashDuration = 0.15f;
    [SerializeField] float _dashCooldown = 4f;
    private float _dashTime = 0f;
    private bool _canDash = true;
    private bool _isDashing;
    private float _dashTimer;
    private Vector3 _dashDirection;


    [Header("Gravité")]
    [SerializeField] private float _fallMultiplier = 2.5f;     // chute plus rapide
    [SerializeField] private float _lowJumpMultiplier = 2f;    // saut plus court si on relâche la touche

    private Vector2 _moveDirection;
    private Rigidbody rb;

    private bool _isGrounded;
    private bool _jumpHeld;
    private int _airJumpsLeft;
    private float _coyoteTimer;
    private float _jumpBufferTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onMoveEvent += OnMove;
        InputListener.onJumpEvent += OnJump;
        InputListener.onDashEvent += OnDash;
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        if (_jumpBuy == true)
        {
            if (context.started)
            {
                _jumpBufferTimer = _jumpBufferTime;
                _jumpHeld = true;
            }
            else if (context.canceled)
            {
                _jumpHeld = false;
            }
        }
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

        _isDashing = true;
        _dashTimer = _dashDuration;
        _dashTime = _dashCooldown;
        _canDash = false;

    }

    private void HandleDash()
    {
        if(_isDashing)
        {
            _dashTimer -= Time.fixedDeltaTime;
            if(_dashTimer <= 0f) _isDashing = false;
        }

        if(!_canDash)
        {
            _dashTime -= Time.fixedDeltaTime;
            if(_dashTime <= 0f) _canDash = true;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
        CheckGround();
        HandleJump();
        HandleDash();
        ApplyBetterGravity();
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

    private void Moving()
    {
        if (_isDashing)
        {
            rb.linearVelocity = new Vector3(_dashDirection.x * _dashForce, 0f, _dashDirection.z * _dashForce);
            return;
        }

        Vector3 dir = GetMoveDirection();
        rb.linearVelocity = new Vector3(dir.x * _speed, rb.linearVelocity.y, dir.z * _speed);
    }

    private void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * _rayDownDistance, Color.green);
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _rayDownDistance, _groundLayer, QueryTriggerInteraction.Ignore);

        if (_isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            _coyoteTimer = _coyoteTime;
            _airJumpsLeft = _maxAirJumps;
        }
        else
        {
            _coyoteTimer -= Time.fixedDeltaTime;
        }
    }

    private void HandleJump()
    {
        _jumpBufferTimer -= Time.fixedDeltaTime;

        if (_jumpBufferTimer <= 0f) return;

        if (_coyoteTimer > 0f)
        {
            Jump();
            _coyoteTimer = 0f;
        }
        else if (_airJumpsLeft > 0)
        {
            Jump();
            _airJumpsLeft--;
        }
    }


    private void Jump()
    {
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(v.x, _jumpForce, v.z);
        _jumpBufferTimer = 0f;
    }

    private void ApplyBetterGravity()
    {
        float vy = rb.linearVelocity.y;

        if (vy < 0f)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (vy > 0f && !_jumpHeld)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }


}
