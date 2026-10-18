using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement Settings")]
    private float _speed = 0f;//in Theory the amount of meters you'd want it to move during the acceleration and decceleration
    [SerializeField] private float _baseSpeed = 45f;//in Theory the amount of meters you'd want it to move during the acceleration and decceleration
    [SerializeField] private float _sprintSpeed = 55f;
    [SerializeField][Tooltip("Time it takes to accelerate to max speed")] private float _accelTime = 1f;
    [SerializeField][Tooltip("Time it takes to deccelerate to 0 ")] private float _deccelTime = 1.5f;
    [SerializeField][Tooltip("the Force of the jump")] private float _jumpForce = 100f;
    private float _acceleration;

    private float _decceleration;
    private Vector2 movementInput;
    private bool hasJumped = false;
    private float _movementSpeed;
    float _currentGravity = 0f;

    private Camera _camera;
    private CharacterController _controller;

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Header("Camera Settings")]
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 70f;

    Vector2 rotation = Vector2.zero;

    private void Awake()
    {
        _speed = _baseSpeed;
        _acceleration = _speed / MathF.Pow(_accelTime, 2);
        _decceleration = _speed / MathF.Pow(_deccelTime, 2);
        _camera = Camera.main; // Get the main Camera

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
    {
        HandleMovement();
        HandleCameraFOV();
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        float xVal = value.Get<Vector2>().x;
        float yVal = value.Get<Vector2>().y;

        rotation.x += xVal * sensitivity;
        rotation.y += yVal * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.rotation = xQuat;
        _camera.transform.localRotation = yQuat;
    }

    private void OnJump(InputValue value)
    {
        hasJumped = value.isPressed;
    }
    private void OnSprint(InputValue value)
    {
        if (value.isPressed)
            _speed = _sprintSpeed;
        else
            _speed = _baseSpeed;
    }

    private void HandleMovement()
    {
        if (movementInput == Vector2.zero)
            _movementSpeed = Mathf.Lerp(_movementSpeed, 0f, _decceleration * Time.fixedDeltaTime);
        else//every frame add acceleration to the movement speed until it reaches the max speed
            _movementSpeed = Mathf.Lerp(_movementSpeed, _speed, _acceleration * Time.fixedDeltaTime);

        Vector3 movement = transform.TransformDirection(new Vector3(movementInput.x, 0f, movementInput.y)) * _movementSpeed;

        if (_controller.isGrounded)
        {
            _currentGravity = -0.5f;
            if (hasJumped)
            {
                _currentGravity = _jumpForce;
                hasJumped = false;
            }
        }
        else
        {
            _currentGravity += Physics.gravity.y * _acceleration * Time.fixedDeltaTime;
        }
        movement.y = _currentGravity;

        _controller.Move(movement * Time.fixedDeltaTime);
    }

    private void HandleCameraFOV()
    {
        if (_speed == _sprintSpeed)
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 80f, _acceleration * Time.fixedDeltaTime);
        else
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60f, _acceleration * Time.fixedDeltaTime);
    }
}