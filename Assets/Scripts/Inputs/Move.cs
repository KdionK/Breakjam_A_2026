
using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : BuyableBehavior, IDamageable
{
    [SerializeField] Transform _cam;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private SceneManagementSystem _sceneManger;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] Animator _doorAnimator;
    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] AudioClip _playerJump;
    [SerializeField] AudioClip _playerDeath;
    [SerializeField] AudioClip _playerHit;
    [SerializeField] AudioClip _playerLand;
    [Header("buyChecks")]

    [SerializeField] public bool _jumpBuy = false;


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

    [Header("Death/Respawn")]
    [SerializeField] Transform _respawnPoint;
    [SerializeField] float _respawnTime = 0.2f;
    private bool _killed;
    private bool _murder;

    bool _isRespawning = false;
    [Header("Regen")]
    [SerializeField] float _timeBeforeRgen = 3f;
    [SerializeField] int _regenAmount = 5;
    [SerializeField] float _tickWait = 0.3f;
    private Coroutine _regenRoutine;
    private bool _hitRecently = false;
    private float _regenTimer = 0;

    [Header("Gravité")]
    [SerializeField] private float _fallMultiplier = 2.5f;     // chute plus rapide
    [SerializeField] private float _lowJumpMultiplier = 2f;    // saut plus court si on relâche la touche

    [Header("Upgrades")]
    [SerializeField] private float _upgradeSpeedAmount = 2.5f;
    [SerializeField] private int _upgradeHealthAmount = 30;

    [Header("IFrames")]
    [SerializeField] private float _frameTime = 0.1f;

 

    private bool _invincible = false;
    private bool _dashInvincible = false;


    private Vector2 _moveDirection;
    private Rigidbody rb;
    private HealthComponent _healthComponent;

    public bool _isGrounded;
    private bool _jumpHeld;
    private int _airJumpsLeft;
    private float _coyoteTimer;
    private float _jumpBufferTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _initialSpeed = _speed;
        _audioSource = GetComponent<AudioSource>();
        InputListener.onMoveEvent += OnMove;
        InputListener.onJumpEvent += OnJump;
        rb = GetComponent<Rigidbody>();
        _healthComponent = GetComponent<HealthComponent>();
        if (BattleStats.Instance) BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    private void DeathTest()
    {
        _healthComponent.ModifyHealth(20);
        _healthBar.SetVisualHealth(_healthComponent.CurrentHealth);
        // HandleDeath();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            DeathTest();
            
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_jumpBuy == true)
        {
            if (context.started)
            {
                if (SoundManagerMino.Instance) SoundManagerMino.Instance.PlaySound(_playerJump,_audioSource, 0.3f );
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
        Regen();

        if (_isDashing)
        {
            _dashInvincible = true;

        }
        else
        {
            _dashInvincible = false;

        }
        

        if (_healthComponent.CurrentHealth <= 0)
        {
            HandleDeath();
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
        if (!rb.isKinematic)
        {
            rb.linearVelocity = new Vector3(dir.x * _speed, rb.linearVelocity.y, dir.z * _speed);
        }
    }

    private void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * _rayDownDistance, Color.green);
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _rayDownDistance, _groundLayer, QueryTriggerInteraction.Ignore);
        if (_isGrounded) _boostTimer = 0f;

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
        if (_sliding)
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
        if (_invincible || _isRespawning || _dashInvincible) return dinfo;
        SoundManagerMino.Instance.PlaySound(_playerHit, _audioSource);
        HealthComponent _healthCompoent = GetComponent<HealthComponent>();
        _healthCompoent.ModifyHealth((int)damage);
        if (_healthBar != null)
        {
            _healthBar.SetVisualHealth(_healthComponent.CurrentHealth);
        }
        if (_regenRoutine != null) StopCoroutine(_regenRoutine);

        _regenRoutine = StartCoroutine(Regen());
        StartCoroutine(IFrames());

        return dinfo;
    }

    // private void Regen()
    // {
    //     if(_healthComponent.CurrentHealth >= _healthComponent.FullHealth)
    //     {
    //         _regenTimer = 0;
    //         return;
    //     }
    //     _regenTimer += Time.deltaTime;

    //     if(_regenTimer >= _timeBeforeRgen)
    //     {
    //         _healthComponent.RegenHealth(_regenAmount);
    //         _healthBar.SetVisualHealth(_healthComponent.CurrentHealth);
    //     }
    // }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(_timeBeforeRgen);
        while (_healthComponent.CurrentHealth < _healthComponent.FullHealth)
        {
            _healthComponent.RegenHealth(_regenAmount);
            _healthBar.SetVisualHealth(_healthComponent.CurrentHealth);

            // Delay between each tick
            yield return new WaitForSeconds(_tickWait);
        }
    }

    public void HandleDeath()
    {
        Debug.Log("Handle Death");
        if (_isRespawning) return;
        SoundManagerMino.Instance.PlaySound(_playerDeath, _audioSource);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        _doorAnimator.SetTrigger("Open");
        ResetEnnemies();
        _isRespawning = true;
        _killed = true;
        Debug.Log("HandleDeath");
        _killed = true;
        // Zero velocity BEFORE going kinematic (setting it on a kinematic body is ignored/warns)
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        _isDashing = false;
        _sliding = false;

        rb.position = _respawnPoint.position;
        rb.rotation = _respawnPoint.rotation;
        transform.SetPositionAndRotation(_respawnPoint.position, _respawnPoint.rotation);

        _jumpHeld = false;

        // Hold here (fade, death animation, sound, etc. would go in this window)
        _healthComponent.SetHealth();
        _healthBar.SetVisualHealth(_healthComponent.CurrentHealth);
        yield return new WaitForSeconds(_respawnTime);
        _killed = false;
        rb.isKinematic = false;
        _isRespawning = false;
    }

    IEnumerator IFrames()
    {
        _invincible = true;
        // Debug.Log("IFrames starting");
        yield return new WaitForSeconds(_frameTime);
        _invincible = false;
        // Debug.Log("IFrames finished");
    }

    void ResetEnnemies()
    {
        GameObject[] Ennemies = GameObject.FindGameObjectsWithTag("Ennemy");
        foreach (GameObject ennemy in Ennemies)
        {
            Destroy(ennemy);
        }
        
        
    }




    public DamageProposalContext ModifyOutgoingDamage(DamageProposalContext ctx)
    {
        return null;
    }

    public HealthComponent GetHealth()
    {
        return null;
    }

    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Player) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                break;
            case UpgradeCategory.Mesh:
                break;
            case UpgradeCategory.Sound:
                Debug.Log("buying player sound");
                SoundManagerMino.Instance.SetPlayer(15);
                break;
            case UpgradeCategory.Health:
                UpgradeHealth();
                break;
            case UpgradeCategory.Speed:
                UpgradeSpeed();
                break;
        }
    }

    public void UpgradeHealth()
    {
        _healthComponent.AddHealth(_upgradeHealthAmount, true);
    }

    public void UpgradeSpeed()
    {
        _speed += _upgradeSpeedAmount;
        _initialSpeed = _speed;
    }


}
