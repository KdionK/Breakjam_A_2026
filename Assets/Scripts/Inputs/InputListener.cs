using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> onRightClickEvent;
    public void onRightClick(InputAction.CallbackContext context)
    {
        onRightClickEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onLeftClickEvent;
    public void onLeftClick(InputAction.CallbackContext context)
    {
        onLeftClickEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onMoveEvent;
    public void onMove(InputAction.CallbackContext context)
    {
        onMoveEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onMouseMoveEvent;
    public void onMouseMove(InputAction.CallbackContext context)
    {
        onMouseMoveEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onConsoleCommandPressed;
    public void onConsoleCommandPressedEvent(InputAction.CallbackContext context)
    {
        onConsoleCommandPressed?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onJumpEvent;

    public void onJump(InputAction.CallbackContext context)
    {
        onJumpEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onDashEvent;
    public void onDash(InputAction.CallbackContext context)
    {
        onDashEvent?.Invoke(context);
    }
    
    public static event Action<InputAction.CallbackContext> onSlideEvent;
    public void onSlide(InputAction.CallbackContext context)
    {
        onSlideEvent?.Invoke(context);
    }
    
    public static event Action<InputAction.CallbackContext> onPress1Event;
    public void onPress1(InputAction.CallbackContext context)
    {
        onPress1Event?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onPress2Event;
    public void onPress2(InputAction.CallbackContext context)
    {
        onPress2Event?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onPress3Event;
    public void onPress3(InputAction.CallbackContext context)
    {
        onPress3Event?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onPress4Event;
    public void onPress4(InputAction.CallbackContext context)
    {
        onPress4Event?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onPressQEvent;
    public void onPressQ(InputAction.CallbackContext context)
    {
        onPressQEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onPressREvent;
    public void onPressR(InputAction.CallbackContext context)
    {
        onPressREvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onScrollUpEvent;
    public void onScrollUp(InputAction.CallbackContext context)
    {
        onScrollUpEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onScrollDownEvent;
    public void onScrollDown(InputAction.CallbackContext context)
    {
        onScrollDownEvent?.Invoke(context);
    }

    public static event Action<InputAction.CallbackContext> onPressEscEvent;
    public void onPressEsc(InputAction.CallbackContext context)
    {
        onPressEscEvent?.Invoke(context);
    }

}
