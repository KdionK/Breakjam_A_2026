using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapPopUp : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _PopUpTitle;
   [SerializeField] private Image Icon;
   [SerializeField] private TextMeshProUGUI PriceTag;
   [SerializeField] private Slider expirationSlider;
   [SerializeField] [Range(10, 30)] private int expirationTime = 22;

   private PopUpSpawner _parentSpawner;
   private GameImportance _importance;
   private Coroutine _expirationCoroutine = null;
   private const string _PriceTagFormat = "{0}¢";

   public GameImportance Importance => _importance;
   public UnityEvent OnExpire;
   public UnityEvent OnBuy;
   
   
   private void Awake()
   {
       expirationTime = Random.Range(20, 30);
       _expirationCoroutine = StartCoroutine(SetExpirationSlider());
       OnExpire.AddListener(DestroyPopUp);
   }

   public void SetPopUp(string title, Sprite icon, int price, PopUpSpawner parentSpawner)
   {
       _PopUpTitle.text = title;
       if (icon != null)
        Icon.sprite = icon;

       PriceTag.text = string.Format(_PriceTagFormat, price);
       _parentSpawner = parentSpawner;
   }


    public static MapPopUp CreatePopUp(PopUpSpawner parent, SO_PopUpComponent popUpcomponent)
    {
        var popUpPrefab = Resources.Load<MapPopUp>("Prefabs/MapPopUp");
        var popUpInstance = Instantiate(popUpPrefab,parent.transform.position,parent.transform.rotation,parent.transform);
        popUpInstance.SetPopUp(popUpcomponent.Title, popUpcomponent.Icon, popUpcomponent.Price, parent);

        parent.HasPopUpOn = true;
        return popUpInstance;
    }

    private IEnumerator SetExpirationSlider()
{
    float currentExpirationLeft = expirationTime;

    expirationSlider.value = 1f;

    while (currentExpirationLeft > 0)
    {
        currentExpirationLeft -= Time.deltaTime;
        expirationSlider.value = currentExpirationLeft / expirationTime;

        yield return null;
    }

    expirationSlider.value = 0f;
    OnExpire?.Invoke();
}

    public void DestroyPopUp()
    {
        if (_expirationCoroutine != null) StopCoroutine(_expirationCoroutine);
            _expirationCoroutine = null;
        _parentSpawner.HasPopUpOn = false;
        Destroy(gameObject);
    }
    
}
