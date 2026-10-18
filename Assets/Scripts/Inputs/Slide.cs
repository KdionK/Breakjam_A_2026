using UnityEngine;
using UnityEngine.InputSystem;


public class Slide : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] Move _moveScript;
    [SerializeField] float _slideSpeed = 10f;
    [SerializeField] Transform _slideCamPos;
    [SerializeField] float _slideDuration = 0.3f;
    [SerializeField] float _slideCooldown = 1f;
    [SerializeField] bool _slideBuy = true;
    [SerializeField] float _camSmoothSpeed = 10f; // higher = snappier

    private Vector3 _camDefaultLocalPos;
    private float _slideTime = 0f;
    private bool _canSlide = true;
    private float _slideTimer;
    private Vector3 _slideDirection;
    private Vector2 _moveDirection;
    private Rigidbody rb;



    void Start()
    {
        InputListener.onSlideEvent += OnSlide;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleSlide();
    }

    void LateUpdate()
    {
        camMovement();
    }

    private void OnSlide(InputAction.CallbackContext context)
    {
        if (!_slideBuy || !_canSlide) return;

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
        if (_moveScript._sliding)
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
        if(_moveScript._sliding)
        {
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
