using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinematicScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textDisplay;
    [SerializeField] private TextAlignment _textAlignment = TextAlignment.Center;

    [SerializeField] private string[] _introTexts;
    [Tooltip("Time in seconds between each letter being displayed.")]
    [SerializeField] private float _typingSpeed = 0.05f;

    [SerializeField] private string nextScene;


    private Coroutine _introTextCoroutine;
    private int _currentTextIndex = 0;


    private void OnEnable()
    {
        _introTextCoroutine = StartCoroutine(TypeText(_introTexts[0]));
    }

    private void OnAny(InputValue value)
    {
        if (value.isPressed && _introTextCoroutine != null)
        {
            StopCoroutine(_introTextCoroutine);
            _introTextCoroutine = null;
            _textDisplay.text = _introTexts[_currentTextIndex];
        }
        else if (value.isPressed && _introTextCoroutine == null)
        {
            _currentTextIndex++;
            if (_currentTextIndex < _introTexts.Length)
            {
                _introTextCoroutine = StartCoroutine(TypeText(_introTexts[_currentTextIndex]));
            }
            else
                SceneManagementSystem.Instance.LoadScene(nextScene);
        }
    }

    private void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Cancel pressed");
            SceneManagementSystem.Instance.LoadScene("GameScene");
        }
    }

    void OnValidate()
    {
        if (_textDisplay != null)
        {
            switch (_textAlignment)
            {
                case TextAlignment.Left:
                    _textDisplay.alignment = TextAlignmentOptions.Left;
                    break;
                case TextAlignment.Center:
                    _textDisplay.alignment = TextAlignmentOptions.Center;
                    break;
                case TextAlignment.Right:
                    _textDisplay.alignment = TextAlignmentOptions.Right;
                    break;
            }
        }
        _textDisplay.text = _introTexts[0];
    }

    private IEnumerator TypeText(string text)
    {
        _textDisplay.text = "";
        foreach (char letter in text.ToCharArray())
        {
            _textDisplay.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }
        _introTextCoroutine = null;
    }

}

enum TextAlignment
{
    Left,
    Center,
    Right
}