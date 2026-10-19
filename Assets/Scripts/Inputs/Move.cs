
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : BuyableBehavior, IDamageable
{
    [SerializeField] Transform _cam;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private SceneManagementSystem _sceneManger;
    [Header("buyChecks")]
    [SerializeField] bool _jumpBuy = true;


    [Header("base movement")]
    private float _initialSpeed;
    [SerializeField] float _speed = 5;
    [Header("Jump")]
    [SerializeField] float _jumpForce = 3;
    [SerializeField] private int _maxAirJumps = 0;
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private float _jumpBufferTime = 0.1f;    // saut mémorisé si pressé juste avant d'atterrir
    [SerializeField] float _rayDownDistance = 5f;
    [Header("Dash")]
    public bool _isDashing = false;

    [Header("SlideJump/Boost")]
    public bool _sliding = false;
    [SerializeField] float _slideJumpSpeed = 10;
    [SerializeField] float _slideJumpBoostTime = 0.6f;
    private Vector3 _boostDir;
    private float _boostTimer;



    [Header("Gravité")]
    [SerializeField] private float _fallMultiplier = 2.5f;     // chute plus rapide
    [SerializeField] private float _lowJumpMultiplier = 2f;    // saut plus court si on relâche la touche

    private Vector2 _moveDirection;
    private Rigidbody rb;

    public bool _isGrounded;
    private bool _jumpHeld;
    private int _airJumpsLeft;
    private float _coyoteTimer;
    private float _jumpBufferTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _initialSpeed = _speed;
        InputListener.onMoveEvent += OnMove;
        InputListener.onJumpEvent += OnJump;
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


    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
        CheckGround();
        HandleJump();
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
            return;
        }

        if (_sliding)
        {
            return;
        }

        if (_boostTimer > 0f)
        {
            _boostTimer -= Time.fixedDeltaTime;
            float t = Mathf.Clamp01(_boostTimer / _slideJumpBoostTime);
            _speed = Mathf.Lerp(_initialSpeed, _slideJumpSpeed, t);
        }
        else
        {
            _speed = _initialSpeed;
        }


        Vector3 dir = GetMoveDirection();
        rb.linearVelocity = new Vector3(dir.x * _speed, rb.linearVelocity.y, dir.z * _speed);
    }

    private void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * _rayDownDistance, Color.green);
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _rayDownDistance, _groundLayer, QueryTriggerInteraction.Ignore);
        if(_isGrounded) _boostTimer = 0f;

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
        if(_sliding)
        {
            _boostTimer = _slideJumpBoostTime;
            _sliding = false;
        }
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


    public DamageInfo TakeDamage(GameObject source, float damage)
    {
        DamageInfo dinfo = new DamageInfo();
        GetComponent<HealthComponent>().ModifyHealth((int)damage);
        return dinfo;
    }

    private void HandleDeath()
    {
        
    }

    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        return null;
    }

    public HealthComponent GetHealth()
    {
        return null;
    }
}
