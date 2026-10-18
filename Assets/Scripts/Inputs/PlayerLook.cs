using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerLook : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float xSensitivity = 30f;
    [SerializeField] float ySensitivity = 30f;
    private Vector2 _moveDirection;

    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onMouseMoveEvent += OnMouseMove;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = _moveDirection.x;
        float mouseY = _moveDirection.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
