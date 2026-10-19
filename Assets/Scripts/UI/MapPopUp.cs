using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UIElements.Slider;

public class MapPopUp : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _PopUpTitle;
   [SerializeField] private Image Icon;
   [SerializeField] private TextMeshProUGUI PriceTag;
   [SerializeField] private Slider expirationSlider;
   [SerializeField] [Range(10, 30)] private int expirationTime = 22;

   

   private const string _PriceTagFormat = "{0}¢";
   
   public UnityEvent OnExpire;
   public UnityEvent OnBuy;
   private Coroutine _expirationCoroutine = null;
   
   private void Awake()
   {
       expirationTime = Random.Range(20, 30);
       _expirationCoroutine = StartCoroutine(SetExpirationSlider());
       OnExpire.AddListener(DestroyPopUp);
   }

   public void SetPopUp(string title, Sprite icon, int price)
   {
        _PopUpTitle.text = title;
        Icon.sprite = icon;
        PriceTag.text = string.Format(_PriceTagFormat, price);
   }

    public static MapPopUp CreatePopUp(PopUpSpawner parent, SO_PopUpComponent popUpcomponent)
    {
        var popUpPrefab = Resources.Load<MapPopUp>("Prefabs/MapPopUp");
        var popUpInstance = Instantiate(popUpPrefab,parent.transform.position,Quaternion.identity,parent.transform);
        popUpInstance.SetPopUp(popUpcomponent.Title, popUpcomponent.Icon, popUpcomponent.Price);
        return popUpInstance;
    }

    private IEnumerator SetExpirationSlider()
    {
        float currentExpirationLeft = expirationSlider.value;
        expirationSlider.value = currentExpirationLeft/expirationTime;

        while (currentExpirationLeft > 0)
        {
            currentExpirationLeft -= Time.deltaTime;
            expirationSlider.value = currentExpirationLeft/expirationTime;
            yield return new WaitForEndOfFrame();
        }
        OnExpire?.Invoke();
    }

    public void DestroyPopUp()
    {
        if (_expirationCoroutine != null) StopCoroutine(_expirationCoroutine);
            _expirationCoroutine = null;
        Destroy(gameObject);
    }
    
}
