using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New PopUp Component", menuName = "ScriptableObjects/PopUpComponent", order = 1)]
public class SO_PopUpComponent : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price;
    
    [SerializeField] private UnitType unitType;
    [SerializeField] private UpgradeCategory categoryType;
    
    [SerializeField] private GameImportance gameImportance = 0;

    public string Title => title;
    public Sprite Icon => icon;
    public int Price => price;
}

enum GameImportance
{
    Base,
    Proto,
    GoodGame
}
