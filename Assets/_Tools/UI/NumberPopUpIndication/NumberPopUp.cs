using UnityEngine;
using UnityEngine.UI;

public class NumberPopUp : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI numberText;
    [SerializeField] private Image[] _decorations;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float lifetime = 1f;

    private float timer;

    public void SetNumber(int number)
    {
        numberText.text = number.ToString();
        timer = 0f;
    }

    private void Update()
    {
        // Move the pop-up upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (_decorations.Length > 0)
        {
            foreach (Image deco in _decorations)
            {
                Color color1 = deco.color;
                color1.a -= fadeSpeed * Time.deltaTime;
                deco.color = color1;
            }
        }

        // Fade out the text
        Color color = numberText.color;
        color.a -= fadeSpeed * Time.deltaTime;
        numberText.color = color;

        // Destroy the pop-up after its lifetime
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
