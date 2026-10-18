using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onRightClickEvent += OnClick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("Click");
    }
}
