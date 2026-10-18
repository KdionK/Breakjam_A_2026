
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [Header("base movement")]
    [SerializeField] float _speed = 5;
    [Header("Jumps")]
    [SerializeField] float _jumpForce = 3;
    [SerializeField] float _gravity = 20f;
    [SerializeField] bool _jumpBuy = true;
    [SerializeField] bool _jumped = false;
    [SerializeField] private int _maxAirJumps = 0;
    [SerializeField] private float _coyoteTime = 0.1f;
     [SerializeField] private float _jumpBufferTime = 0.1f;    // saut mémorisé si pressé juste avant d'atterrir
    [SerializeField] float _rayDownDistance = 5f;
    private bool _isGrounded = false;
    private Vector2 _moveDirection;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onMoveEvent += OnMove;
        InputListener.onJumpEvent += OnJump;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = _cam.forward;
        Vector3 right = _cam.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * _moveDirection.y + right * _moveDirection.x;

        rb.linearVelocity = new Vector3(dir.x * _speed, 0f, dir.z * _speed);

        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * _rayDownDistance, Color.green);
        if(Physics.Raycast(transform.position,Vector3.down,out hit, _rayDownDistance))
        {
            if(hit.transform.tag == "Floor")
            {
                Debug.Log("hit the floor");
                _isGrounded = true;
                _jumped = false;
            }
        }
        else
        {
            _isGrounded = false;
        }


    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("moving");
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _jumped = true;
    }
}
