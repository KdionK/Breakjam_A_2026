using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private HealthComponent _healthComponent;

    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _healthDelayImage;
    [SerializeField] private Color _blinkColor;
    [SerializeField] NumberPopUpSpawner _numberPopUpSpawner;




    [Header("Shake Animation Parameter")]
    [SerializeField] private int _shakeAmount = 5;
    [SerializeField] private float _shakeAnimationDuration = 0.5f;
    [SerializeField] private int _shakesSpeed = 20;
    [SerializeField] private TextMeshProUGUI _currentText;
    [SerializeField] private TextMeshProUGUI _fullText;

    private RectTransform _healthRectTransform;
    private RectTransform _healthDelayRectTransform;

    private Coroutine _delayCoroutine = null;
    private float _baseWidth;

    private float _baseHealthHeight;
    private float _baseHealthDelayHeight;
    private Coroutine _roulerCoroutine = null;

    private bool _showTempHit;
    private int _tempHealth;
    private Color _backHealthColor;

    void Awake()
    {
        _healthRectTransform = _healthImage.GetComponent<RectTransform>();
        _healthDelayRectTransform = _healthDelayImage.GetComponent<RectTransform>();

        _baseHealthHeight = _healthRectTransform.rect.height;
        _baseHealthDelayHeight = _healthDelayRectTransform.rect.height;

        _baseWidth = _healthRectTransform.rect.width;
        _backHealthColor = _healthDelayImage.color;
    }

    public void SetTempHealthLoss(int lowerHpBy)
    {
        _showTempHit = true;
        _tempHealth = _healthComponent.CurrentHealth - lowerHpBy;
        float newWidth = _tempHealth * _baseWidth / _healthComponent.FullHealth;
        _healthRectTransform.sizeDelta = new(newWidth, _baseHealthHeight);
    }

    public void UnsetTempHealthLoss()
    {
        _showTempHit = false;
        _tempHealth = _healthComponent.CurrentHealth;
        float newWidth = _healthComponent.CurrentHealth * _baseWidth / _healthComponent.FullHealth;
        _healthRectTransform.sizeDelta = new(newWidth, _baseHealthHeight);
    }

    private void FixedUpdate()
    {
        if (_showTempHit)
        {
            float t = (Mathf.Sin(Time.time * 8) + 1f) * 0.5f;
            Color currentColor = Color.Lerp(_backHealthColor, _blinkColor, t);
            _healthDelayImage.GetComponent<Image>().color = currentColor;
        }
    }

    public void SetUpHealthBar(HealthComponent hp = null)
    {
        if (hp != null)
            _healthComponent = hp;

        _currentText.text = _healthComponent.CurrentHealth.ToString();
        _fullText.text = _healthComponent.FullHealth.ToString();

        ResetHealthBar();
    }

    // private void SpawnNumberPopUp(int healthLoss)
    // {
    //     _numberPopUpSpawner.SpawnNumberPopUp(healthLoss);
    // }

    public void SetVisualHealth(int newHealth)
    {

        // SpawnNumberPopUp(_healthComponent.CurrentHealth - newHealth);
        float newWidth = _healthComponent.CurrentHealth * _baseWidth / _healthComponent.FullHealth;
        _healthRectTransform.sizeDelta = new(newWidth, _baseHealthHeight);
        StartCoroutine(COShakeHealthBar());

        if (_roulerCoroutine != null)
            StopCoroutine(_roulerCoroutine);
        _roulerCoroutine = StartCoroutine(COLowerTextLife());
        if (_delayCoroutine != null)
            StopCoroutine(_delayCoroutine);

        _delayCoroutine = StartCoroutine(CODelayBar(newWidth));
    }

    public void ResetHealthBar()
    {
        _healthRectTransform.sizeDelta = new(_baseWidth, _baseHealthHeight);
        _healthDelayRectTransform.sizeDelta = new(_baseWidth, _baseHealthDelayHeight);

        _currentText.text = _healthComponent.CurrentHealth.ToString();
        _fullText.text = _healthComponent.FullHealth.ToString();
        if (_roulerCoroutine != null)
        {
            StopCoroutine(_roulerCoroutine);
            _roulerCoroutine = null;
        }
        if (_delayCoroutine != null)
        {
            StopCoroutine(_delayCoroutine);
            _delayCoroutine = null;
        }
    }

    private IEnumerator COShakeHealthBar()
    {
        Vector3 basePosition = GetComponent<RectTransform>().anchoredPosition3D;
        for (int i = 0; i < _shakeAmount; i++)
        {
            float compteurSecondes = 0;
            Vector3 RandomPos = new(basePosition.x + Random.Range(-15f, 15f), basePosition.y + Random.Range(-15f, 15f), basePosition.z + Random.Range(-2f, 2f));
            while (compteurSecondes <= _shakeAnimationDuration / _shakeAmount)
            {
                GetComponent<RectTransform>().anchoredPosition3D = Vector3.MoveTowards(GetComponent<RectTransform>().anchoredPosition3D, RandomPos, _shakesSpeed * Time.fixedDeltaTime);
                yield return null;
                compteurSecondes += Time.deltaTime;
            }
            GetComponent<RectTransform>().anchoredPosition3D = RandomPos;
        }
        GetComponent<RectTransform>().anchoredPosition3D = basePosition;
    }

    private IEnumerator CODelayBar(float newWidth)
    {
        Vector2 targetSize = new(newWidth, _baseHealthDelayHeight);
        yield return new WaitForSeconds(0.3f);
        while (Vector2.Distance(_healthDelayRectTransform.sizeDelta, targetSize) > 0.5f)
        {
            _healthDelayRectTransform.sizeDelta = Vector2.MoveTowards(_healthDelayRectTransform.sizeDelta, targetSize, 100 * Time.fixedDeltaTime);
            yield return null;
        }
        _healthDelayRectTransform.sizeDelta = targetSize;
        _delayCoroutine = null;
    }

    private IEnumerator COLowerTextLife()
    {
        int vieAffiches = int.Parse(_currentText.text);
        while (vieAffiches > _healthComponent.CurrentHealth)
        {
            vieAffiches--;
            _currentText.text = vieAffiches.ToString();
            yield return new WaitForSeconds(0.1f);//attente entre chaque mise à jour
        }
        _currentText.text = vieAffiches.ToString(); ;
        _roulerCoroutine = null;
    }
}
