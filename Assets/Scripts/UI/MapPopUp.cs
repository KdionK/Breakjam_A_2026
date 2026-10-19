using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapPopUp : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _PopUpTitle;
   [SerializeField] private Image Icon;
   [SerializeField] private TextMeshProUGUI PriceTag;

   private const string _PriceTagFormat = "{0}¢";

    public void SetPopUp(string title, Sprite icon, int price)
    {
        _PopUpTitle.text = title;
        Icon.sprite = icon;
        PriceTag.text = string.Format(_PriceTagFormat, price);
    }
}
