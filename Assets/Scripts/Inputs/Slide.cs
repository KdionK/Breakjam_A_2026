using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class Slide : BuyableBehavior
{

    [SerializeField] Transform _cam;
    [SerializeField] CinemachineCamera _cinemachineCamera;
    [SerializeField] Move _moveScript;
    [SerializeField] float _slideSpeed = 10f;
    [SerializeField] Transform _slideCamPos;
    [SerializeField] Transform _slideCamRoot;
    [SerializeField] float _slideDuration = 0.3f;
    [SerializeField] float _slideCooldown = 1f;
    private AudioSource _playerAudio;
    [SerializeField] AudioClip _slideSound;

    [Header("Camera")]
    [SerializeField] float _camSmoothSpeed = 10f; // higher = snappier
    [SerializeField] float _targetFov = 90f;
    private float _initialFov;
    private float _arriveThreshold = 0.98f;
    private Vector3 _camDefaultLocalPos;
    private float _slideTime = 0f;
    private bool _canSlide = true;
    private float _slideTimer;
    private Vector3 _slideDirection;
    private Vector2 _moveDirection;
    private Rigidbody rb;



    void Start()
    {
        _camDefaultLocalPos = _slideCamRoot.transform.localPosition;
        _initialFov = _cinemachineCamera.Lens.FieldOfView;
        _playerAudio = GetComponent<AudioSource>();
        InputListener.onSlideEvent += OnSlide;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleSlide();
        if (!_moveScript._sliding)
        {
            _slideTime = 0;
        }
    }

    void LateUpdate()
    {
        camMovement();
    }

    private void OnSlide(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // ignore started / canceled (release)

        if (!_canSlide || !_moveScript._isGrounded || _moveScript._isDashing) return;
        SoundManagerMino.Instance.PlaySound(_slideSound,_playerAudio);

        Vector3 dir = GetMoveDirection();
        if (dir.sqrMagnitude < 0.01f)
        {
            dir = _cam.forward;
            dir.y = 0f;
        }
        _slideDirection = dir.normalized;

        _moveScript._sliding = true;
        _slideTimer = _slideDuration;
        _slideTime = _slideCooldown;
        _canSlide = false;
    }

    private void HandleSlide()
    {
        if (_moveScript._sliding && _moveScript._isGrounded)
        {
            rb.linearVelocity = new Vector3(_slideDirection.x * _slideSpeed, rb.linearVelocity.y, _slideDirection.z * _slideSpeed);
            _slideTimer -= Time.fixedDeltaTime;
            if (_slideTimer <= 0f) _moveScript._sliding = false;
        }

        if (!_canSlide)
        {
            _slideTime -= Time.fixedDeltaTime;
            if (_slideTime <= 0f) _canSlide = true;
        }
    }

    private void camMovement()
    {
        // Target in the camera's parent space, so it works wherever _slideCamPos sits in the hierarchy
        Vector3 target;
        if (_moveScript._sliding)
        {
            // make a smooth FOV transition with a cinemachine camera that ends when the camera move end(smoothly) and start resetting the fov when the camera move is over
            target = _cam.parent.InverseTransformPoint(_slideCamPos.position);
        }
        else
        {
            target = _camDefaultLocalPos;
        }


        // Frame-rate independent smoothing
        float t = 1f - Mathf.Exp(-_camSmoothSpeed * Time.deltaTime);
        _cam.localPosition = Vector3.Lerp(_cam.localPosition, target, t);
    }


    // private void UpdateFov(Vector3 slideLocal)
    // {
    //     Vector3 path = slideLocal - _camDefaultLocalPos;
    //     float sqrLense = path.sqrMagnitude;

    //     float progress = 0f;
    //     if (sqrLense > 0.0001f)
    //     {
    //         progress = Vector3.Dot(_cam.localPosition - _camDefaultLocalPos, path) / sqrLense;
    //         progress = Mathf.Clamp01(progress / _arriveThreshold);
    //     }

    //     var lens = _cinemachineCamera.Lens;
    //     float eased = Mathf.SmoothStep(0f, 1f, progress);
    //     lens.FieldOfView = Mathf.Lerp(_initialFov, _targetFov, eased);
    //     _cinemachineCamera.Lens = lens;
    // }


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
