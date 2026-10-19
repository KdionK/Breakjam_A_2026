using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UIElements.Slider;

public class MapPopUp : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _PopUpTitle;
   [SerializeField] private Image Icon;
   [SerializeField] private TextMeshProUGUI PriceTag;

   private const string _PriceTagFormat = "{0}¢";
   
   [SerializeField] Slider expirationSlider;

   [SerializeField] [Range(10, 30)] private int expirationTime = 22;
   
   private void Awake()
   {
       expirationTime = Random.Range(20, 30);
   }

   public void SetPopUp(string title, Sprite icon, int price)
    {
        _PopUpTitle.text = title;
        Icon.sprite = icon;
        PriceTag.text = string.Format(_PriceTagFormat, price);
    }

    public static MapPopUp CreatePopUp(Transform parent, string title, Sprite icon, int price)
    {
        var popUpPrefab = Resources.Load<MapPopUp>("Prefabs/MapPopUp");
        var popUpInstance = Instantiate(popUpPrefab, parent);
        popUpInstance.SetPopUp(title, icon, price);
        return popUpInstance;
    }

    private IEnumerator SetExpirationSlider()
    {
        float currentExpirationLeft = expirationSlider.value;
        expirationSlider.value = currentExpirationLeft/expirationTime;

        while (currentExpirationLeft > 0)
        {
            currentExpirationLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
