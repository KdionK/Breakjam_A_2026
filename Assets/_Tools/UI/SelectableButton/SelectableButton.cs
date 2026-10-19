using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour
{
    private SelectableButton _currentlyHoveredButton;
    [SerializeField] public Color _buttonColor;

    [SerializeField] private InputActionReference pointerAction;
    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Camera uiCamera;

    private Image _Image;
    [SerializeField] private TextMeshProUGUI _coolDownText;
    [SerializeField] private Image TimerIcon;
    [SerializeField] private HealthBar _hpBar;

    private bool _isOnCooldown;
    private bool _isNotSelected = true;

    private Animator _animator;

    void OnValidate()
    {
        _Image = transform.GetChild(0).GetComponent<Image>();
        _Image.color = _buttonColor;
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _Image = transform.GetChild(0).GetComponent<Image>();

    }

    private void OnEnable()
    {
        clickAction.action.started += OnClickStarted;
        pointerAction.action.performed += OnPointerMoved;
    }

    private void OnDisable()
    {
        clickAction.action.started -= OnClickStarted;
        pointerAction.action.performed -= OnPointerMoved;
    }





    public void StartCooldown(int cooldown)
    {
        _isOnCooldown = true;
        TimerIcon.gameObject.SetActive(true);
        _Image.color = new(_buttonColor.r - 0.4f, _buttonColor.g - 0.4f, _buttonColor.b - 0.4f);
        _coolDownText.text = cooldown.ToString();
        _coolDownText.gameObject.SetActive(true);
    }

    public void UpdateCooldown(int newCooldown)
    {
        _coolDownText.text = newCooldown.ToString();
    }

    public void EndCooldown()
    {
        _isOnCooldown = false;
        TimerIcon.gameObject.SetActive(false);
        _Image.color = _buttonColor;
        _isNotSelected = true;
        _coolDownText.text = "";
        _coolDownText.gameObject.SetActive(false);
    }

    public void SetSelected()
    {
        _isNotSelected = false;
        _Image.color = new(_buttonColor.r - 0.4f, _buttonColor.g - 0.4f, _buttonColor.b - 0.4f);
    }

    public void SetClickable()
    {
        _isNotSelected = true;
        _Image.color = _buttonColor;
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        Vector2 pointerPosition = pointerAction.action.ReadValue<Vector2>();
        if (IsPointerOverObject(pointerPosition) && !_isOnCooldown && _isNotSelected)
        {
            SetSelected();
            //Add Use here;
        }
    }

    private bool IsPointerOverObject(Vector2 pointerPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(panelTransform, pointerPosition, uiCamera);
    }



    private void OnPointerMoved(InputAction.CallbackContext context)
    {
        Vector2 pointerPosition = pointerAction.action.ReadValue<Vector2>();
        if (IsPointerOverObject(pointerPosition))
        {
            _currentlyHoveredButton = this;
            if (!_isOnCooldown && _isNotSelected)
                _animator.SetBool("IsHovering", true);
        }
        else
        {
            if (_currentlyHoveredButton == this)
            {
                _currentlyHoveredButton = null;
                _animator.SetBool("IsHovering", false);
            }
        }

    }
}
